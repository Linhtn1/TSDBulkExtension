using System.Collections;
using System.Data;
using System.Data.Common;
using TSD.BulkExtensions.Metadata;

namespace TSD.BulkExtensions.Streaming;

/// <summary>
/// Streams an entity list to a bulk-copy API as a forward-only <see cref="DbDataReader"/>. One row is materialised at a
/// time, so memory stays flat regardless of list size. An optional trailing integer column carries each row's position
/// in the list so generated keys can be mapped back by index rather than by insertion order.
/// </summary>
public sealed class EntityDataReader<T> : DbDataReader where T : class
{
    /// <summary>Name of the optional position column.</summary>
    public const string IndexColumnName = "__Index";

    private readonly IList<T> _entities;
    private readonly BulkValueContext _values;
    private readonly IReadOnlyList<ColumnMap> _columns;
    private readonly bool _includeIndex;
    private readonly Dictionary<string, int> _ordinals;
    private readonly object?[] _current;
    private int _position = -1;
    private bool _closed;

    /// <summary>Creates a reader over <paramref name="entities"/> exposing <paramref name="columns"/> in order, plus <see cref="IndexColumnName"/> when requested.</summary>
    public EntityDataReader(IList<T> entities, BulkValueContext values, IReadOnlyList<ColumnMap> columns, bool includeIndexColumn)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
        _values = values ?? throw new ArgumentNullException(nameof(values));
        _columns = columns ?? throw new ArgumentNullException(nameof(columns));
        _includeIndex = includeIndexColumn;
        _current = new object?[FieldCount];

        _ordinals = new Dictionary<string, int>(FieldCount, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < _columns.Count; i++)
        {
            _ordinals[_columns[i].ColumnName] = i;
        }

        if (_includeIndex)
        {
            _ordinals[IndexColumnName] = _columns.Count;
        }
    }

    /// <summary>Columns in ordinal order, excluding the position column.</summary>
    public IReadOnlyList<ColumnMap> Columns => _columns;

    /// <summary>Zero-based position of the row last returned by <see cref="Read"/>.</summary>
    public int CurrentIndex => _position;

    /// <inheritdoc />
    public override int FieldCount => _columns.Count + (_includeIndex ? 1 : 0);

    /// <inheritdoc />
    public override int Depth => 0;

    /// <inheritdoc />
    public override bool HasRows => _entities.Count > 0;

    /// <inheritdoc />
    public override bool IsClosed => _closed;

    /// <inheritdoc />
    public override int RecordsAffected => -1;

    /// <inheritdoc />
    public override object this[int ordinal] => GetValue(ordinal);

    /// <inheritdoc />
    public override object this[string name] => GetValue(GetOrdinal(name));

    /// <inheritdoc />
    public override bool Read()
    {
        if (_closed || _position + 1 >= _entities.Count)
        {
            return false;
        }

        _position++;
        var entity = _entities[_position] ?? throw new InvalidOperationException($"Entity at index {_position} is null.");

        for (var i = 0; i < _columns.Count; i++)
        {
            _current[i] = _columns[i].GetProviderValue(entity, _values);
        }

        if (_includeIndex)
        {
            _current[_columns.Count] = _position;
        }

        return true;
    }

    /// <inheritdoc />
    public override bool NextResult() => false;

    /// <inheritdoc />
    public override string GetName(int ordinal)
        => ordinal < _columns.Count ? _columns[ordinal].ColumnName
         : ordinal == _columns.Count && _includeIndex ? IndexColumnName
         : throw new IndexOutOfRangeException();

    /// <inheritdoc />
    public override int GetOrdinal(string name)
        => _ordinals.TryGetValue(name, out var ordinal) ? ordinal : throw new IndexOutOfRangeException($"Column '{name}' not found.");

    /// <inheritdoc />
    public override Type GetFieldType(int ordinal)
        => ordinal < _columns.Count ? _columns[ordinal].ProviderClrType
         : ordinal == _columns.Count && _includeIndex ? typeof(int)
         : throw new IndexOutOfRangeException();

    /// <inheritdoc />
    public override string GetDataTypeName(int ordinal)
        => ordinal < _columns.Count ? _columns[ordinal].StoreType
         : ordinal == _columns.Count && _includeIndex ? "int"
         : throw new IndexOutOfRangeException();

    /// <inheritdoc />
    public override object GetValue(int ordinal)
    {
        EnsureRow();
        return _current[ordinal] ?? DBNull.Value;
    }

    /// <inheritdoc />
    public override int GetValues(object[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        EnsureRow();
        var count = Math.Min(values.Length, FieldCount);
        for (var i = 0; i < count; i++)
        {
            values[i] = _current[i] ?? DBNull.Value;
        }

        return count;
    }

    /// <inheritdoc />
    public override bool IsDBNull(int ordinal)
    {
        EnsureRow();
        return _current[ordinal] is null;
    }

    /// <inheritdoc />
    public override bool GetBoolean(int ordinal) => (bool)GetValue(ordinal);

    /// <inheritdoc />
    public override byte GetByte(int ordinal) => (byte)GetValue(ordinal);

    /// <inheritdoc />
    public override char GetChar(int ordinal) => (char)GetValue(ordinal);

    /// <inheritdoc />
    public override DateTime GetDateTime(int ordinal) => (DateTime)GetValue(ordinal);

    /// <inheritdoc />
    public override decimal GetDecimal(int ordinal) => (decimal)GetValue(ordinal);

    /// <inheritdoc />
    public override double GetDouble(int ordinal) => (double)GetValue(ordinal);

    /// <inheritdoc />
    public override float GetFloat(int ordinal) => (float)GetValue(ordinal);

    /// <inheritdoc />
    public override Guid GetGuid(int ordinal) => (Guid)GetValue(ordinal);

    /// <inheritdoc />
    public override short GetInt16(int ordinal) => (short)GetValue(ordinal);

    /// <inheritdoc />
    public override int GetInt32(int ordinal) => (int)GetValue(ordinal);

    /// <inheritdoc />
    public override long GetInt64(int ordinal) => (long)GetValue(ordinal);

    /// <inheritdoc />
    public override string GetString(int ordinal) => (string)GetValue(ordinal);

    /// <inheritdoc />
    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length)
    {
        var bytes = (byte[])GetValue(ordinal);
        if (buffer is null)
        {
            return bytes.Length;
        }

        var count = (int)Math.Min(length, bytes.Length - dataOffset);
        Array.Copy(bytes, dataOffset, buffer, bufferOffset, count);
        return count;
    }

    /// <inheritdoc />
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length)
    {
        var chars = GetString(ordinal);
        if (buffer is null)
        {
            return chars.Length;
        }

        var count = (int)Math.Min(length, chars.Length - dataOffset);
        chars.CopyTo((int)dataOffset, buffer, bufferOffset, count);
        return count;
    }

    /// <inheritdoc />
    public override IEnumerator GetEnumerator() => new DbEnumerator(this, closeReader: false);

    /// <summary>Minimal schema (name, ordinal, type, nullability) for bulk-copy APIs that inspect the source.</summary>
    public override DataTable GetSchemaTable()
    {
        var schema = new DataTable("SchemaTable") { Locale = System.Globalization.CultureInfo.InvariantCulture };
        schema.Columns.Add(SchemaTableColumn.ColumnName, typeof(string));
        schema.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof(int));
        schema.Columns.Add(SchemaTableColumn.DataType, typeof(Type));
        schema.Columns.Add(SchemaTableColumn.AllowDBNull, typeof(bool));

        for (var i = 0; i < FieldCount; i++)
        {
            var allowNull = i < _columns.Count && _columns[i].IsNullable;
            schema.Rows.Add(GetName(i), i, GetFieldType(i), allowNull);
        }

        return schema;
    }

    /// <inheritdoc />
    public override void Close() => _closed = true;

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _closed = true;
        base.Dispose(disposing);
    }

    private void EnsureRow()
    {
        if (_position < 0)
        {
            throw new InvalidOperationException("No current row. Call Read() first.");
        }
    }
}
