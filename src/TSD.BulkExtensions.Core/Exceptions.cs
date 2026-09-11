namespace TSD.BulkExtensions;

/// <summary>Base exception for failures raised by TSD.BulkExtensions itself (not the database driver).</summary>
public class BulkExtensionsException : Exception
{
    /// <inheritdoc />
    public BulkExtensionsException() { }

    /// <inheritdoc />
    public BulkExtensionsException(string message) : base(message) { }

    /// <inheritdoc />
    public BulkExtensionsException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>Thrown when <see cref="BulkConfig"/> contains an option combination that cannot be executed.</summary>
public sealed class InvalidBulkConfigException : BulkExtensionsException
{
    /// <inheritdoc />
    public InvalidBulkConfigException() { }

    /// <inheritdoc />
    public InvalidBulkConfigException(string message) : base(message) { }

    /// <inheritdoc />
    public InvalidBulkConfigException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>Thrown when no provider adapter is available for the context's database provider.</summary>
public sealed class BulkProviderNotFoundException : BulkExtensionsException
{
    /// <inheritdoc />
    public BulkProviderNotFoundException() { }

    /// <inheritdoc />
    public BulkProviderNotFoundException(string message) : base(message) { }

    /// <inheritdoc />
    public BulkProviderNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
