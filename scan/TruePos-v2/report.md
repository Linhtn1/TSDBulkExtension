# EFCore.BulkExtensions usage scan

Generated: 2026-09-11T13:45:36  
Roots: D:\LinhTN\Projects\TruePos\src  
Files: 8716 .cs, 62 .csproj/.props

## Projects (TFM + relevant packages)

| Project | TFM | Packages |
|---|---|---|
| src\Acen\TSD.POS.Acen.Application\TSD.POS.Acen.Application.csproj | net6.0 |  |
| src\Acen\TSD.POS.Acen.Application.Shared\TSD.POS.Acen.Application.Shared.csproj | net6.0 | Abp.AutoMapper 7.4.0<br>Abp.Web.Common 7.4.0 |
| src\Acen\TSD.POS.Acen.CoreBase\TSD.POS.Acen.CoreBase.csproj | net6.0 | Abp 7.4.0<br>Abp.Zero.Common 7.4.0 |
| src\TSD.POS.Accountings\TSD.POS.Accountings.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Accountings.Shared\TSD.POS.Accountings.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Application\TSD.POS.Application.csproj | net6.0 | Abp.AspNetCore.PerRequestRedisCache 7.4.0<br>Abp.Castle.Log4Net 7.4.0<br>Abp.EntityFrameworkCore.EFPlus 7.4.0<br>Abp.HangFire.AspNetCore 7.4.0<br>Abp.RedisCache 7.4.0 |
| src\TSD.POS.Application.Client\TSD.POS.Application.Client.csproj | netstandard2.0 |  |
| src\TSD.POS.Application.Print\TSD.POS.Application.Print.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Application.Print.Shared\TSD.POS.Application.Print.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Application.Shared\TSD.POS.Application.Shared.csproj | netstandard2.0 | Abp.AutoMapper 7.4.0<br>Abp.Web.Common 7.4.0 |
| src\TSD.POS.BackgroundJobs\TSD.POS.BackgroundJobs.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.CashBooks\TSD.POS.CashBooks.csproj | net6.0 |  |
| src\TSD.POS.CashBooks.Shared\TSD.POS.CashBooks.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Common\TSD.POS.Common.csproj | netstandard2.0 |  |
| src\TSD.POS.Core\TSD.POS.Core.csproj | net6.0 | Abp.AspNetCore.SignalR 7.4.0<br>Abp.AspNetZeroCore 4.1.0<br>Abp.ZeroCore.IdentityServer4.vNext.EntityFrameworkCore 7.4.0<br>Abp.AutoMapper 7.4.0<br>Abp.MailKit 7.4.0<br>Abp.Zero.Ldap 7.4.0 |
| src\TSD.POS.Core.Shared\TSD.POS.Core.Shared.csproj | netstandard2.0 | Abp 7.4.0<br>Abp.Zero.Common 7.4.0 |
| src\TSD.POS.Customers\TSD.POS.Customers.csproj | net6.0 |  |
| src\TSD.POS.Customers.Shared\TSD.POS.Customers.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Delivery\TSD.POS.Delivery.csproj | net6.0 |  |
| src\TSD.POS.Delivery.Shared\TSD.POS.Delivery.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Ecommerces\TSD.POS.Ecommerces.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Ecommerces.Shared\TSD.POS.Ecommerces.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.EntityFrameworkCore\TSD.POS.EntityFrameworkCore.csproj | net6.0 | EFCore.BulkExtensions 6.5.6<br>Abp.EntityFrameworkCore.EFPlus 7.4.0<br>Microsoft.EntityFrameworkCore.SqlServer 6.0.10<br>Microsoft.EntityFrameworkCore.Tools 6.0.10<br>Microsoft.EntityFrameworkCore.Design 6.0.10 |
| src\TSD.POS.GraphQL\TSD.POS.GraphQL.csproj | net6.0 |  |
| src\TSD.POS.HumanResources\TSD.POS.HumanResources.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.HumanResources.Shared\TSD.POS.HumanResources.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.ImportGoods\TSD.POS.ImportGoods.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.ImportGoods.Shared\TSD.POS.ImportGoods.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Managers\TSD.POS.Managers.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Migrator\TSD.POS.Migrator.csproj | net6.0 | Abp.Castle.Log4Net 7.4.0 |
| src\TSD.POS.Mobile.Droid\TSD.POS.Mobile.Droid.csproj |  | Abp<br>Abp.AutoMapper<br>Abp.Web.Common<br>Abp.Zero.Common |
| src\TSD.POS.Mobile.iOS\TSD.POS.Mobile.iOS.csproj |  | Abp<br>Abp.AutoMapper<br>Abp.Web.Common |
| src\TSD.POS.Mobile.Shared\TSD.POS.Mobile.Shared.csproj | netstandard2.0 | Abp 7.4.0<br>Abp.AutoMapper 7.4.0 |
| src\TSD.POS.Products\TSD.POS.Products.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Products.Shared\TSD.POS.Products.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Reports\TSD.POS.Reports.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Reports.Shared\TSD.POS.Reports.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Sales\TSD.POS.Sales.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Sales.Shared\TSD.POS.Sales.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.SocialChannels\TSD.POS.SocialChannels.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.SocialChannels.Shared\TSD.POS.SocialChannels.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.StoreManages\TSD.POS.StoreManages.csproj | net6.0 |  |
| src\TSD.POS.StoreManages.Shared\TSD.POS.StoreManages.Shared.csproj | net6.0 |  |
| src\TSD.POS.SystemConfigurations\TSD.POS.SystemConfigurations.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.SystemConfigurations.Shared\TSD.POS.SystemConfigurations.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Taxes\TSD.POS.Taxes.csproj | net6.0 |  |
| src\TSD.POS.Taxes.Shared\TSD.POS.Taxes.Shared.csproj | net6.0 |  |
| src\TSD.POS.Warehouses\TSD.POS.Warehouses.csproj | net6.0 |  |
| src\TSD.POS.Warehouses.Shared\TSD.POS.Warehouses.Shared.csproj | net6.0 | Abp 7.4.0 |
| src\TSD.POS.Web.Core\TSD.POS.Web.Core.csproj | net6.0 | Abp.AspNetCore.SignalR 7.4.0<br>Abp.AspNetZeroCore.Web 4.1.0<br>Abp.ZeroCore.IdentityServer4.vNext 7.4.0<br>Abp.AspNetCore 7.4.0<br>Abp.HangFire.AspNetCore 7.4.0<br>Abp.RedisCache 7.4.0 |
| src\TSD.POS.Web.Host\TSD.POS.Web.Host.csproj | net6.0 | Abp.Castle.Log4Net 7.4.0<br>Microsoft.EntityFrameworkCore.Design 6.0.10 |
| src\TSD.POS.Web.Mvc\TSD.POS.Web.Mvc.csproj | net6.0 | Abp.Castle.Log4Net 7.4.0<br>Microsoft.EntityFrameworkCore.Design 6.0.10 |
| src\TSD.POS.Web.Public\TSD.POS.Web.Public.csproj | net6.0 | Microsoft.EntityFrameworkCore.Design 6.0.10<br>Abp.Castle.Log4Net 7.4.0<br>Abp.AspNetCore.SignalR 7.4.0 |
| src\WPF\Presentation\TSD.POS.WPF\TSD.POS.WPF.Main.csproj | net8.0-windows |  |
| src\WPF\Provider\TSD.POS.KTTS.IProvider\TSD.POS.KTTS.IProvider.csproj | net8.0 |  |
| src\WPF\Provider\TSD.POS.WPF.Provider\TSD.POS.WPF.Provider.csproj | net8.0 |  |
| src\WPF\Util\TSD.POS.WPF.ColorFont\TSD.POS.WPF.ColorFont.csproj | netcoreapp3.1 |  |
| src\WPF\Util\TSD.POS.WPF.Controls\TSD.POS.WPF.Controls.csproj | netcoreapp3.1 |  |
| src\WPF\Util\TSD.POS.WPF.Language\TSD.POS.WPF.Language.csproj | netcoreapp3.1 |  |
| src\WPF\Util\TSD.POS.WPF.Util\TSD.POS.WPF.Util.csproj | netcoreapp3.1 | Abp.AutoMapper 5.13.0 |
| src\WPF\Util\TSD.POS.WPF.XRHelper\TSD.POS.WPF.XRHelper.csproj | netcoreapp3.1 |  |
| src\WPF\Util\TSD.Util\TSD.Util.csproj | netcoreapp3.1 | Abp.Castle.Log4Net 5.13.0 |

## DB providers

| Name | Hits | Files |
|---|---|---|
| SqlServer | 2 | 1 |

## Bulk method call sites

| Name | Hits | Files |
|---|---|---|
| BulkInsert | 4 | 3 |
| BulkInsertAsync | 317 | 110 |
| BulkUpdate | 2 | 1 |
| BulkUpdateAsync | 233 | 89 |
| BulkDelete | 2 | 1 |
| BulkDeleteAsync | 98 | 49 |
| BulkInsertOrUpdate | 2 | 1 |
| BulkInsertOrUpdateAsync | 42 | 25 |
| BulkInsertOrUpdateOrDelete | 2 | 1 |
| BulkInsertOrUpdateOrDeleteAsync | 2 | 1 |
| BulkRead | 2 | 1 |
| BulkReadAsync | 2 | 1 |
| Truncate | 7 | 6 |
| TruncateAsync | 1 | 1 |
| BatchDeleteAsync | 28 | 6 |

## BulkConfig options referenced

| Name | Hits | Files |
|---|---|---|
| BatchSize | 26 | 9 |
| IncludeGraph | 62 | 36 |
| PreserveInsertOrder | 20 | 15 |
| PropertiesToInclude | 82 | 36 |
| SetOutputIdentity | 140 | 69 |
| UseTempDB | 1 | 1 |
| WithHoldlock | 79 | 25 |

## EF model features affecting bulk mapping

| Name | Hits | Files |
|---|---|---|
| DefaultValue | 1 | 1 |
| GlobalQueryFilter | 1 | 1 |
| SoftDelete_ABP | 408 | 408 |

## Summary

- using EFCore.BulkExtensions: 8 lines in 8 files
- BulkRepository / IBulkRepository references: 1133 lines in 212 files
- BulkConfig instantiations / lambdas: 59 lines in 6 files

## Bulk method call sites (samples)

### BulkInsert (4 hits)

```
src\TSD.POS.Application\_QuayBanHang\DDonHangs\OrdersStockProcessManager.cs:257  _donHangChiTietLoHanRepository.BulkInsert(listBatchDetailOrders, opt => opt.WithHoldlock = false);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:73  GetContext().BulkInsert(entities, bulkConfig, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:77  GetContext().BulkInsert(entities, bulkAction, progress, type);
src\TSD.POS.Sales\DDonHangs\DDonHangsWriteAppService.cs:1888  _donHangChiTietLoHanRepository.BulkInsert(listBatchDetailOrders);
```

### BulkInsertAsync (317 hits)

```
src\TSD.POS.Accountings\Accounting\AccountingEntries\AccountingEntriesAppService.cs:456  await _accountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(accountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Accountings\Accounting\AcenAutoBusinessEntities\AcenAutoBusinessEntitiesManager.cs:184  await _acenAutoBusinessEntityRepository.BulkInsertAsync(defaultDatas, opt => opt.IncludeGraph = true);
src\TSD.POS.Accountings\Accounting\AcenSAccounts\AcenSAccountsAppService.cs:144  await _acenSAccountValidator.AcenSAccountRepository.BulkInsertAsync(insertedItems);
src\TSD.POS.Accountings\Accounting\DWorkShiftRecords\DWorkShiftRecordsAppService.cs:636  await _workShiftRecordEmployeeRepository.BulkInsertAsync(dto.EmployeeIds.Select(x => new WorkShiftRecordEmployee
src\TSD.POS.Accountings\Accounting\DWorkShiftRecords\DWorkShiftRecordsAppService.cs:673  await _workShiftRecordMoneyRepository.BulkInsertAsync(recordMoneys, opt => opt.IncludeGraph = true);
src\TSD.POS.Accountings\STaiKhoanNganHang\BankAccountManager.cs:822  await _sBranchBankAccountsRepository.BulkInsertAsync(insEntities);
src\TSD.POS.Application\Accounting\AccountingEntries\CashBooks\CashBooksAccountingEntriesManager.cs:49  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\Accounting\AccountingEntries\ComboProcessings\ComboProcessingsAccountingEntriesManager.cs:102  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ConsumerGoodsIssues\ConsumerGoodsIssuesManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\DWorkShiftRecords\DWorkShiftRecordsEntriesManager.cs:54  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertedAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\DWorkShiftRecords\DWorkShiftRecordsEntriesManager.cs:95  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertedAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\GoodsDisposals\GoodsDisposalsManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\GoodsTransfers\GoodsTransfersEntriesManager.cs:53  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ImportGoods\ImportGoodsAccountingEntriesManager.cs:67  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\InventoryCounts\InventoryCountsEntriesManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\OrdersAccountingEntriesManager.cs:151  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\PurchaseOrdersAccountingEntriesManager.cs:53  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\OrderVouchers\OrderVouchersEntriesManager.cs:100  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\PreImportGoods\PreImportGoodsAccountingEntriesManager.cs:48  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ReturnPurchaseGoods\ReturnPurchaseGoodsAccountingEntriesManager.cs:60  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:97  await _acenSyncHistoriesRepository.BulkInsertAsync(items, opt => opt.IncludeGraph = true);
src\TSD.POS.Application\Authorization\HAAuthentication\HAApiManager.cs:118  await _dUserCentralRepository.BulkInsertAsync(listDUserCentral, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1158  await _userRoleExtendRepository.BulkInsertAsync(listUserRolesExtend);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1162  await _dUserChiNhanhRepository.BulkInsertAsync(listUserBranch);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1459  await _dUserAccessTimeRepository.BulkInsertAsync(listDUserAccessTime);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:2266  await _permissionsExtendRepository.BulkInsertAsync(listUserPermissionExtend);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:2919  await _dUserChiNhanhRepository.BulkInsertAsync(listUserChiNhanh);
src\TSD.POS.Application\BaoCaos\BaoCaoBanHang\OrderReportManager.cs:314  await _donHangSanPhamRepository.BulkInsertAsync(listDonHangSanPhams, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:695  await _eivMauSoKyHieuRepository.BulkInsertAsync(insertedEivMauSoKyHieus, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:724  await _eivSHinhThucThanhToansRepository.BulkInsertAsync(insertEivSHinhThucThanhToans, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2039  await EcoOrderRepository.BulkInsertAsync(ecoOrders.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2045  await EcoOrderReturnRepository.BulkInsertAsync(ecoOrderReturns.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(f ...
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:258  await EcoProductRepository.BulkInsertAsync(listProduct.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:398  await EcoProductRepository.BulkInsertAsync(listProduct.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:547  await EcoProductRepository.BulkInsertAsync(ecoProducts.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:520  await EivCauHinhTruongMoRongRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:584  await EivMauSoKyHieuRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:636  await EivSHinhThucThanhToanRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:727  await _eivSDVTRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:777  await EivSCauHinhLPhiRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
```

### BulkUpdate (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:152  GetContext().BulkUpdate(entities, bulkConfig, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:157  GetContext().BulkUpdate(entities, bulkAction, progress, type);
```

### BulkUpdateAsync (233 hits)

```
src\TSD.POS.Accountings\Accounting\AccountingEntries\AccountingEntriesAppService.cs:493  await _accountingEntryValidator.AccountingEntryRepository.BulkUpdateAsync(accountingEntries);
src\TSD.POS.Accountings\STaiKhoanNganHang\BankAccountManager.cs:797  await _sBranchBankAccountsRepository.BulkUpdateAsync(entities, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:94  await _acenSyncHistoryDetailsRepository.BulkUpdateAsync(existedHistories);
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:105  await _acenSyncHistoriesRepository.BulkUpdateAsync(items.Where(x => x.RefId.HasValue).ToList());
src\TSD.POS.Application\Authorization\HAAuthentication\HAApiManager.cs:97  await _userRepository.BulkUpdateAsync(misingPhoneNumberUsers);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1240  await _userRepository.BulkUpdateAsync(users);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1288  await _userRepository.BulkUpdateAsync(users);
src\TSD.POS.Application\BaoCaos\BaoCaoBanHang\OrderReportManager.cs:95  await DDonHangRepository.BulkUpdateAsync(orders).ConfigureAwait(false);
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:234  await _thongTinCuaHangRepository.BulkUpdateAsync(new List<ThongTinCuaHang>
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:245  await _sThongTinDangKyRepository.BulkUpdateAsync(new List<SThongTinDangKy>
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:295  await _dDigitalSignatureInfoRepository.BulkUpdateAsync(dDigitalSignatureInfos, opt => opt.PropertiesToInclude = new List<string> { nameof(DDigitalSignatureInfo.IsDefault) });
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:1123  await _dHoaDonDienTuRepository.BulkUpdateAsync(batch, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:1317  await _dHoaDonDienTuRepository.BulkUpdateAsync(invoices.Select(x => new DHoaDonDienTu
src\TSD.POS.Application\Ecommerce\DEcoOrderChecks\DEcoOrderChecksManager.cs:187  await _dEcoOrderCheckRepository.BulkUpdateAsync(dEcoOrderCheckUpdates);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:291  await _dEcoOrderCrossCheckRepository.BulkUpdateAsync(ecoOrderCrossCheck);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2038  await EcoOrderRepository.BulkUpdateAsync(ecoOrders.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2044  await EcoOrderReturnRepository.BulkUpdateAsync(ecoOrderReturns.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Products\CopyEcoProductManager.cs:741  await EcoProductRepository.BulkUpdateAsync(listEcoProducts).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:257  await EcoProductRepository.BulkUpdateAsync(listProduct.Where(x => x.Id > 0).ToList());
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:397  await EcoProductRepository.BulkUpdateAsync(listProduct.Where(x => x.Id > 0).ToList());
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:401  await EcoProductRepository.BulkUpdateAsync(ecoProducts, opt =>
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:540  await EcoProductRepository.BulkUpdateAsync(listEcoProducts, opt => opt.PropertiesToInclude = new List<string> { nameof(EcoProduct.UnitId), nameof(EcoProduct.ErrorMessage) });
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:546  await EcoProductRepository.BulkUpdateAsync(ecoProducts.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:667  await DHoaDonDienTuRepository.BulkUpdateAsync(eivInfos.Select(x => new DHoaDonDienTu
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:1491  await EivMauSoKyHieuRepository.BulkUpdateAsync(eivTemplates, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:1548  await DHoaDonDienTuRepository.BulkUpdateAsync(einvoices, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1178  await _dHoaDonDienTuRepository.BulkUpdateAsync(items.Where(x => x.Id > 0).ToList());
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1490  await _eivOrderErrorRepository.BulkUpdateAsync(eivOrderErrorAddOrUpdates.Where(x => x.Id > 0).ToList());
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:2346  await _dDonHangRepository.BulkUpdateAsync(orders.Distinct().Select(x => new DDonHang
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:2427  await _dDonHangRepository.BulkUpdateAsync(noneTaxAuthorityOrders.Select(x => new DDonHang
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:326  await _vNPTInvoiceRepository.BulkUpdateAsync(updatedInvoices, opt => opt.PropertiesToInclude = new List<string> { nameof(VNPTInvoice.TaxStatus) });
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:832  await _dDonHangRepository.BulkUpdateAsync(orders.Select(x => new DDonHang
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:1130  await _eivOrderErrorRepository.BulkUpdateAsync(eivOrderErrorAddOrUpdates.Where(x => x.Id > 0).ToList());
src\TSD.POS.Application\PurchaseInvoices\PurchaseInvoicesManager.cs:393  await _eivDPurchaseInvoiceMappingProductRepository.BulkUpdateAsync(updateMappingProducts, opt =>
src\TSD.POS.Application\Staxes\StaxesAppService.cs:273  await _sTaxRepository.BulkUpdateAsync(updateSTaxes, opt =>
src\TSD.POS.Application\Staxes\StaxesManager.cs:189  await _crmSanPhamRepository.BulkUpdateAsync(crmProducts, opt =>
src\TSD.POS.Application\Staxes\StaxesManager.cs:231  await _posSanPhamRepository.BulkUpdateAsync(posProducts, opt =>
src\TSD.POS.Application\Staxes\StaxesManager.cs:273  await _sanPhamCoreRepository.BulkUpdateAsync(coreProducts, opt =>
src\TSD.POS.Application\Staxes\StaxesManager.cs:315  await _sCostOfSaleRepository.BulkUpdateAsync(sCostOfSales, opt =>
src\TSD.POS.Application\Staxes\StaxesManager.cs:351  await _donHangChiTietBulkRepository.BulkUpdateAsync(orderDetails, opt =>
```

### BulkDelete (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:53  GetContext().BulkDelete(entities, bulkConfig, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:58  GetContext().BulkDelete(entities, bulkAction, progress, type);
```

### BulkDeleteAsync (98 hits)

```
src\TSD.POS.Accountings\Accounting\AcenSAccounts\AcenSAccountsAppService.cs:143  await _acenSAccountValidator.AcenSAccountRepository.BulkDeleteAsync(allExistedItems);
src\TSD.POS.Accountings\Accounting\DWorkShiftRecords\DWorkShiftRecordsAppService.cs:631  await _workShiftRecordEmployeeRepository.BulkDeleteAsync(entity.WorkShiftRecordEmployees);
src\TSD.POS.Accountings\Accounting\DWorkShiftRecords\DWorkShiftRecordsAppService.cs:634  await _workShiftRecordMoneyRepository.BulkDeleteAsync(entity.WorkShiftRecordMoneys);
src\TSD.POS.Accountings\STaiKhoanNganHang\BankAccountManager.cs:819  if (delEntities.Count > 0) await _sBranchBankAccountsRepository.BulkDeleteAsync(delEntities);
src\TSD.POS.Accountings\STaiKhoanNganHang\STaiKhoanNganHangsesAppService.cs:289  await _sBranchBankAccountsRepository.BulkDeleteAsync(branchBankAccounts);
src\TSD.POS.Application\Accounting\AccountingEntries\CashBooks\CashBooksAccountingEntriesManager.cs:570  await AccountingEntryValidator.AccountingEntryRepository.BulkDeleteAsync(deletedAccountingEntries);
src\TSD.POS.Application\Accounting\AccountingEntries\ComboProcessings\ComboProcessingsAccountingEntriesManager.cs:62  await AccountingEntryValidator.AccountingEntryRepository.BulkDeleteAsync(accountingEntries);
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\OrdersAccountingEntriesManager.cs:128  await AccountingEntryValidator.AccountingEntryRepository.BulkDeleteAsync(orderAccountingEntries);
src\TSD.POS.Application\Accounting\AccountingEntries\OrderVouchers\OrderVouchersEntriesManager.cs:76  await AccountingEntryValidator.AccountingEntryRepository.BulkDeleteAsync(existedAccountingEntries);
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:159  await _acenSyncHistoryDetailsRepository.BulkDeleteAsync(deleteDetails);
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:162  await _acenSyncHistoriesRepository.BulkDeleteAsync(allDeleteRequest);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:747  await _permissionsExtendRepository.BulkDeleteAsync(permissionUserChiNhanhs);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:792  await _dUserChiNhanhRepository.BulkDeleteAsync(oldChiNhanhs);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1049  await _permissionsExtendRepository.BulkDeleteAsync(permissionUserChiNhanhs);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1072  await _dUserChiNhanhRepository.BulkDeleteAsync(userChiNhanhs);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1099  await _dUserChiNhanhRepository.BulkDeleteAsync(deleteUserBranchs);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:1422  await _dUserAccessTimeRepository.BulkDeleteAsync(oldTimes);
src\TSD.POS.Application\Authorization\Users\UserAppService.cs:2263  await _permissionsExtendRepository.BulkDeleteAsync(listUserPermissionExtendRemove);
src\TSD.POS.Application\Authorization\Users\Profile\ProfileAppService.cs:291  await _userTokenRepository.BulkDeleteAsync(tokens);
src\TSD.POS.Application\BaoCaos\BaoCaoBanHang\OrderReportManager.cs:115  await _donHangSanPhamRepository.BulkDeleteAsync(products);
src\TSD.POS.Application\DataVersions\DataVersionsAppService.cs:232  await _dataVersionRepository.BulkDeleteAsync(dataversions);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:157  await _dEcoOrderCrossCheckDetailRepository.BulkDeleteAsync(crossCheckDetailRemovals);
src\TSD.POS.Application\Ecommerce\Products\CopyEcoProductManager.cs:745  if (listEcoOrderError.Count > 0) await EcoOrderErrorRepository.BulkDeleteAsync(listEcoOrderError).ConfigureAwait(false);
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1436  await _eivOrderErrorRepository.BulkDeleteAsync(eivOrderErrors);
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1495  await _eivOrderErrorRepository.BulkDeleteAsync(eivOrderErrors.Where(x => !eivOrderErrorAddOrUpdates.Select(e => e.Id).Contains(x.Id) && successfullyOrders.Select(o => o.Id).Contains(x.OrderId)).ToList());
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:231  await _eivOrderErrorRepository.BulkDeleteAsync(eivOrderErrors);
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:1135  await _eivOrderErrorRepository.BulkDeleteAsync(eivOrderErrors.Where(x => successfullyOrders.Select(o => o.Id).Contains(x.OrderId)).ToList());
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamNhaCungCaps\CrmSanPhamNhaCungCapManager.cs:124  await CrmSanPhamNhaCungCapRepository.BulkDeleteAsync(crmSanPhamNhaCungCaps, opt => opt.BatchSize = 100);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:62  await PosSanPhamRepository.BulkDeleteAsync(deletePosSanPhams);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:276  await PosSanPhamRepository.BulkDeleteAsync(deletePosSanPhams);
src\TSD.POS.Application\_QuanTriNguoiDung\NguoiDung\UserDevicesAppService.cs:239  await _userDeviceRepository.BulkDeleteAsync(userDevices);
src\TSD.POS.Application\_QuanTriNguoiDung\ThongTinChung\CoCauToChuc\HaSCoCauToChucs\HaSCoCauToChucsAppService.cs:1078  await _sBranchBankAccountsRepository.BulkDeleteAsync(branchBankAccount);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangManager.cs:736  await NhomKhachHangKhachHangRepository.BulkDeleteAsync(listOldNhomKhachHangKH);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangManager.cs:970  await SoQuyManager.DObjectLedgerManager.DObjectLedgerRepository.BulkDeleteAsync(objectLedgers);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\OrderPaymentManager.cs:779  await DObjectLedgerManager.DObjectLedgerRepository.BulkDeleteAsync(deletedObjectLedgers);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\OrdersCashBookProcessManager.cs:260  await _soQuyManager.DGLVoucherCrossEntryDetailManager.DGLVoucherCrossEntryDetailRepository.BulkDeleteAsync(deletedDGLVoucherCrossEntryDetails);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\OrdersCashBookProcessManager.cs:283  await _dObjectLedgerManager.DObjectLedgerRepository.BulkDeleteAsync(oldOrderObjectLedgers);
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:161  await DObjectLedgerRepository.BulkDeleteAsync(listObjectLedger, config);
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:179  await DObjectLedgerRepository.BulkDeleteAsync(listObjectLedgers);
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:2034  await ChungTuPhieuThuChiesRepository.BulkDeleteAsync(listDeleted);
```

### BulkInsertOrUpdate (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:92  GetContext().BulkInsertOrUpdate(entities, bulkAction, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:97  GetContext().BulkInsertOrUpdate(entities, bulkConfig, progress, type);
```

### BulkInsertOrUpdateAsync (42 hits)

```
src\TSD.POS.Application\Ecommerce\DEcoOrderChecks\DEcoOrderChecksSyncManager.cs:132  await _dEcoOrderCheckRepository.BulkInsertOrUpdateAsync(listOrderCheck).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:192  await _dEcoOrderCrossCheckDetailRepository.BulkInsertOrUpdateAsync(orderCrossCheckDetails).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:428  await _dEcoOrderCrossCheckRepository.BulkInsertOrUpdateAsync(ecoOrderCrossChecks, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:157  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:225  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:282  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\_Kho\DPhieuChuyenKhos\DPhieuChuyenKhosManager.cs:350  await _posSanPhamsManager.CrmSanPhamRepository.BulkInsertOrUpdateAsync(response.sanPhams, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_Kho\DTheKhos\DTheKhoManager.cs:593  await SanPhamKhoRepository.BulkInsertOrUpdateAsync(productStocks);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:84  await PosSanPhamRepository.BulkInsertOrUpdateAsync(posSanPhams.SelectMany(x => x.PosSanPhams).ToList(), opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\_NhapHang\SanPham\ThietLapSanPhams\ThietLapSanPhamManager.cs:94  await ThietLapSanPhamRepository.BulkInsertOrUpdateAsync(thietLapSanPhams);
src\TSD.POS.Application\_NhapHang\SanPham\ThietLapSanPhams\ThietLapSanPhamManager.cs:110  await ThietLapSanPhamRepository.BulkInsertOrUpdateAsync(thietLapSanPhams, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\DLichSuKhoaSo\DLichSuKhoaSosesAppService.cs:90  await _dLichSuKhoaSosRepository.BulkInsertOrUpdateAsync(lichSuHienCos);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\OrdersStockProcessManager.cs:849  await _sanPhamKhoRepository.BulkInsertOrUpdateAsync(stockProducts);
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:136  await DObjectLedgerRepository.BulkInsertOrUpdateAsync(newObjectLedgers);
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:176  await DGLVoucherCrossEntryDetailRepository.BulkInsertOrUpdateAsync(output, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:207  await DGLVoucherCrossEntryDetailRepository.BulkInsertOrUpdateAsync(output, opt => opt.SetOutputIdentity = true).ConfigureAwait(false);
src\TSD.POS.Customers\CrmSNhomKhachHangs\CrmSNhomKhachHangsAppService.cs:547  await _crmSNhomKhachHangsManager.CrmSNhomKhachHangRepository.BulkInsertOrUpdateAsync(new List<CrmSNhomKhachHang>() { crmSNhomKhachHang }, opt => { opt.IncludeGraph = true; opt.PreserveInsertOrder = true; opt.SetOutputIde ...
src\TSD.POS.Delivery\CrossCheck\CrossChecksAppService.cs:1038  await _crossCheckRepository.BulkInsertOrUpdateAsync(new List<CrossCheck> { crossCheck }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Ecommerces\ProductTemplates\EcoProductTemplatesAppService.cs:403  await _ecoProductTemplateRepository.BulkInsertOrUpdateAsync(listSubProducts);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:102  await GetContext().BulkInsertOrUpdateAsync(entities, bulkConfig, progress, type, cancellationToken);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:107  await GetContext().BulkInsertOrUpdateAsync(entities, bulkAction, progress, type, cancellationToken);
src\TSD.POS.HumanResources\BangHoaHong\CrmSanPhamHoaHongs\CrmSanPhamHoaHongsAppService.cs:256  await _crmSanPhamHoaHongBulkRepository.BulkInsertOrUpdateAsync(listCrmSanPhamHoaHong);
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2243  await _dPhieuTraHangRepository.BulkInsertOrUpdateAsync(new List<DPhieuTraHang> { phieuTraHang }, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2249  await _phieuTraHangChiTietRepository.BulkInsertOrUpdateAsync(phieuTraHangChiTietCapNhats, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2257  await _phieuTraHangChiPhiNhapHoanLaiRepository.BulkInsertOrUpdateAsync(chiPhiHoanLais);
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:3255  await _sanPhamKhoLoHanBulkRepository.BulkInsertOrUpdateAsync(sanPhamKhoLoHans).ConfigureAwait(false);
src\TSD.POS.ImportGoods\PhieuNhapHangs\ImportGoodsManager.cs:1210  await _crmSerialRepository.BulkInsertOrUpdateAsync(serialInsert, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.ImportGoods\PhieuNhapHangs\ImportGoodsManager.cs:1256  await NhapHangManager.DPhieuNhapKhoRepository.BulkInsertOrUpdateAsync(new List<DPhieuNhapKho> { dPhieuNhapHang }, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.ImportGoods\PhieuNhapHangs\ImportGoodsManager.cs:1271  await NhapHangManager.PhieuNhapKhoChiTietRepository.BulkInsertOrUpdateAsync(phieuChiTietCapNhats, opt => { opt.IncludeGraph = true; });
src\TSD.POS.ImportGoods\PhieuNhapHangs\ImportGoodsManager.cs:1280  await _phieuNhapKhoChiPhiNhapHangRepository.BulkInsertOrUpdateAsync(dPhieuNhapHang.PhieuNhapKhoChiPhiNhapHangs);
src\TSD.POS.ImportGoods\PhieuNhapHangs\ImportGoodsManager.cs:1431  await _crmSanPhamBangGiaBanRepository.BulkInsertOrUpdateAsync(crmSanPhamBangGiaBans, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.ImportGoods\PhieuNhapHangs\PhieuNhapHangAppServices.cs:435  await _nhapHangManager.DPhieuNhapKhoRepository.BulkInsertOrUpdateAsync(new List<DPhieuNhapKho> { dPhieuNhapHang }, opt => { opt.IncludeGraph = true; });
src\TSD.POS.ImportGoods\PhieuNhapHangs\PhieuNhapHangAppServices.cs:1398  await _nhapHangManager.DPhieuNhapKhoRepository.BulkInsertOrUpdateAsync(listImportGoods, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
src\TSD.POS.Products\CrmSanPhams\CrmSanPhamsUpdateAppService.cs:2863  await _crmSanPhamManager.SanPhamKhoLoHanRepository.BulkInsertOrUpdateAsync(sanPhamKhoLoHans, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Sales\DDonHangs\DDonHangsWriteAppService.cs:2132  await _sanPhamKhoRepository.BulkInsertOrUpdateAsync(sanPhamKhos);
src\TSD.POS.Sales\DDonHangs\DDonHangsWriteAppService.cs:2372  await _sanPhamKhoRepository.BulkInsertOrUpdateAsync(sanPhamKhos);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:839  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:2872  await _donHangCouponRepository.BulkInsertOrUpdateAsync(listOrderCoupons);
src\TSD.POS.Sales\PosExts\PosExtsAppService.cs:33  await _posExtsManager.PosExtRepository.BulkInsertOrUpdateAsync(posExts, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
```

### BulkInsertOrUpdateOrDelete (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:112  GetContext().BulkInsertOrUpdateOrDelete(entities, bulkConfig, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:117  GetContext().BulkInsertOrUpdateOrDelete(entities, bulkAction, progress, type);
```

### BulkInsertOrUpdateOrDeleteAsync (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:122  await GetContext().BulkInsertOrUpdateOrDeleteAsync(entities, bulkAction, progress, type, cancellationToken);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:127  await GetContext().BulkInsertOrUpdateOrDeleteAsync(entities, bulkConfig, progress, type, cancellationToken);
```

### BulkRead (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:132  GetContext().BulkRead(entities, bulkAction, progress, type);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:137  GetContext().BulkRead(entities, bulkConfig, progress, type);
```

### BulkReadAsync (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:142  await GetContext().BulkReadAsync(entities, bulkAction, progress, type, cancellationToken);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:147  await GetContext().BulkReadAsync(entities, bulkConfig, progress, type, cancellationToken);
```

### Truncate (7 hits)

```
src\TSD.POS.Application\Security\ForgotPasswordOtpSender.cs:128  return message?.Truncate(MaxLoggedMessageLength);
src\TSD.POS.Application.Shared\Utilities\Number\NumberUtilities.cs:34  numb = Math.Truncate(numb);
src\TSD.POS.Core\Authorization\Users\User.cs:115  PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
src\TSD.POS.Core\Authorization\Users\User.cs:139  VerifyDeviceCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:172  GetContext().Truncate<TEntity>(type);
src\TSD.POS.Sales\DDonHangs\Importing\ImportDonHangDataSource.cs:836  if (parsed < 0 || !Enum.IsDefined(typeof(ThueBanHangMacDinh), (int)parsed) || parsed != decimal.Truncate(parsed))
src\TSD.POS.Web.Core\Helpers\TSDNumberFormat.cs:74  decimal PhanNguyen = Math.Truncate(number * phanThapPhan);
```

### TruncateAsync (1 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:177  await GetContext().TruncateAsync<TEntity>(type, cancellationToken);
```

### BatchDeleteAsync (28 hits)

```
src\TSD.POS.Application\Auditing\ExpiredAuditLogDeleterWorker.cs:148  AsyncHelper.RunSync(() => _auditLogRepository.BatchDeleteAsync(expression));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:496  await EivCauHinhTruongMoRongRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:554  await _chiNhanhMauSoKyHieuRepository.BatchDeleteAsync(x => deletedIds.Contains(x.EivMauSoKyHieuId.Value));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:555  await EivMauSoKyHieuRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:612  await EivSHinhThucThanhToanRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:703  await _eivSDVTRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:753  await EivSCauHinhLPhiRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:878  await DHoaDonDienTuRepository.BatchDeleteAsync(x => x.MaDonVi.Equals(input.Auth.MaDonVi) && listDeleteEivId.Contains(x.EivId));
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:2241  await _eivSDVTRepository.BatchDeleteAsync(x => maDonVis.Contains(x.MaDonVi));
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:2243  await _chiNhanhMauSoKyHieuRepository.BatchDeleteAsync(x => listMauSoKyHieu.Contains((int)x.EivMauSoKyHieuId));
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:2244  await _eivMauSoKyHieuRepository.BatchDeleteAsync(x => listMauSoKyHieu.Contains(x.Id));
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:2245  await _eivSHinhThucThanhToanRepository.BatchDeleteAsync(x => maDonVis.Contains(x.MaDonVi));
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:2246  await _eivCauHinhTruongMoRongRepository.BatchDeleteAsync(x => maDonVis.Contains(x.MaDonVi));
src\TSD.POS.Application\_QuanTriNguoiDung\ThongTinChung\CoCauToChuc\HaSCoCauToChucs\HaSCoCauToChucsAppService.cs:1886  await _chiNhanhMauSoKyHieuRepository.BatchDeleteAsync(x => deletedIds.Contains(x.Id));
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:795  await _dPhieuQuaTangManager.PhieuQuaTangChiNhanhRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:796  await _dPhieuQuaTangManager.PhieuQuaTangNhomKhachHangRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:797  await _dPhieuQuaTangManager.PhieuQuaTangKenhBanHangRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:798  await _dPhieuQuaTangManager.PhieuQuaTangUserBanHangRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:799  await _dPhieuQuaTangManager.PhieuQuaTangUserSuaRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:985  await _dPhieuQuaTangManager.PhieuQuaTangThuongHieuRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:986  await _dPhieuQuaTangManager.PhieuQuaTangNhomSanPhamRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:987  await _dPhieuQuaTangManager.PhieuQuaTangSanPhamRepository.GetAll().Where(e => e.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:1003  .BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:1004  await _dPhieuQuaTangManager.DThaoTacPhieuQuaTangRepository.GetAll().Where(d => d.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PhieuQuaTang\DPhieuQuaTangs\DPhieuQuaTangsAppService.cs:1005  await _dPhieuQuaTangManager.PhieuQuaTangDanhSachPhieuRepository.GetAll().Where(d => d.DPhieuQuaTangId == dPhieuQuaTangId).BatchDeleteAsync();
src\TSD.POS.Sales\PromotionProcessings\PromotionProcessingsAppService.cs:1077  await _promotionProcessingScopeRepository.BatchDeleteAsync(x => ids.Contains(x.PromotionProcessingId));
src\TSD.POS.Sales\PromotionProcessings\PromotionProcessingsAppService.cs:1078  await _promotionProcessingsManager.PromotionProcessingDetailRepository.BatchDeleteAsync(x => ids.Contains(x.PromotionProcessingId));
src\TSD.POS.Sales\PromotionProcessings\PromotionProcessingsAppService.cs:1079  await _promotionProcessingsManager.PromotionProcessingRepository.BatchDeleteAsync(x => ids.Contains(x.Id));
```

## BulkConfig options (samples)

### BatchSize (26 hits)

```
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2038  await EcoOrderRepository.BulkUpdateAsync(ecoOrders.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2039  await EcoOrderRepository.BulkInsertAsync(ecoOrders.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2044  await EcoOrderReturnRepository.BulkUpdateAsync(ecoOrderReturns.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2045  await EcoOrderReturnRepository.BulkInsertAsync(ecoOrderReturns.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(f ...
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:546  await EcoProductRepository.BulkUpdateAsync(ecoProducts.Where(x => x.Id > 0).ToList(), opt => { opt.BatchSize = TsdConst.BatchSize_1000; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:547  await EcoProductRepository.BulkInsertAsync(ecoProducts.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:674  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:196  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:238  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:280  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:322  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:357  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:392  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\Staxes\StaxesManager.cs:427  opt.BatchSize = TsdConst.BatchSize_10000;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamNhaCungCaps\CrmSanPhamNhaCungCapManager.cs:124  await CrmSanPhamNhaCungCapRepository.BulkDeleteAsync(crmSanPhamNhaCungCaps, opt => opt.BatchSize = 100);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:280  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:1598  opt.BatchSize = 1000;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:1747  opt.BatchSize = BatchSize_1000;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:3760  await _dDonHangExtBulkRepository.BulkInsertAsync(orderExtBatche.Value, opt => { opt.BatchSize = TsdConst.BatchSize_10000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Ecommerces\Orders\EcoOrdersAppService.cs:1435  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Ecommerces\Orders\EcoOrdersAppService.cs:2592  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Ecommerces\Orders\EcoOrdersAppService.cs:2598  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Ecommerces\Orders\EcoOrdersAppService.cs:2604  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:493  opt.BatchSize = TsdConst.BatchSize_1000;
src\TSD.POS.Taxes\TVANRegistrations\TVANManager.cs:166  opt.BatchSize = TsdConst.BatchSize_100;
src\TSD.POS.Taxes\TVANRegistrations\TVANManager.cs:218  opt.BatchSize = TsdConst.BatchSize_100;
```

### IncludeGraph (62 hits)

```
src\TSD.POS.Accountings\Accounting\AcenAutoBusinessEntities\AcenAutoBusinessEntitiesManager.cs:184  await _acenAutoBusinessEntityRepository.BulkInsertAsync(defaultDatas, opt => opt.IncludeGraph = true);
src\TSD.POS.Accountings\Accounting\DWorkShiftRecords\DWorkShiftRecordsAppService.cs:673  await _workShiftRecordMoneyRepository.BulkInsertAsync(recordMoneys, opt => opt.IncludeGraph = true);
src\TSD.POS.Application\Accounting\AcenSyncHistories\AcenSyncHistoriesManager.cs:97  await _acenSyncHistoriesRepository.BulkInsertAsync(items, opt => opt.IncludeGraph = true);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:428  await _dEcoOrderCrossCheckRepository.BulkInsertOrUpdateAsync(ecoOrderCrossChecks, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
src\TSD.POS.Application\SThongTinDangKys\Seed\MauInCreator.cs:41  await SMauInRepository.BulkInsertAsync(mauIns, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\Warehouses\InventoryCountsManager.cs:776  await _phieuKiemKhoRepository.BulkUpdateAsync(new List<DPhieuKiemKho> { input }, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_Kho\DPhieuChuyenKhos\DPhieuChuyenKhosManager.cs:350  await _posSanPhamsManager.CrmSanPhamRepository.BulkInsertOrUpdateAsync(response.sanPhams, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_Kho\DPhieuChuyenKhos\DPhieuChuyenKhosManager.cs:734  await _posSanPhamsManager.CrmSanPhamRepository.BulkUpdateAsync(products, opt => opt.IncludeGraph = true);
src\TSD.POS.Application\_NhapHang\PhieuNhapHangs\NhapHangManager.cs:1441  await DDatHangNhapRepository.BulkUpdateAsync(new List<DDatHangNhap> { input }, opt => { opt.IncludeGraph = isUpdateValue; });
src\TSD.POS.Application\_NhapHang\PhieuNhapHangs\NhapHangManager.cs:1590  await DPhieuTraHangRepository.BulkUpdateAsync(new List<DPhieuTraHang> { input }, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:204  opt.IncludeGraph = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:394  opt.IncludeGraph = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:610  opt.IncludeGraph = true;
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:222  opt.IncludeGraph = true;
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:228  opt.IncludeGraph = true;
src\TSD.POS.Application\_NhapHang\SanPham\ThietLapSanPhams\ThietLapSanPhamManager.cs:110  await ThietLapSanPhamRepository.BulkInsertOrUpdateAsync(thietLapSanPhams, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_QuayBanHang\DDonHangs\CreateOrdersProcessManager.cs:75  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_QuayBanHang\DDonHangs\CreateOrdersProcessManager.cs:173  opt.IncludeGraph = true;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:1536  await _phieuChuyenKhoRepository.BulkInsertAsync(transferVoucherBatch.Value, opt => { opt.IncludeGraph = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:3751  await _dDonHangChiTietRepository.BulkInsertAsync(preOrderDetailBatch.Value, opt => { opt.IncludeGraph = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:621  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dTraHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:1382  await _dDonHangRepository.BulkInsertAsync(returnGoodsOrders, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:4120  await DCaPaymentRepository.BulkInsertAsync(listPhieuChi, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:4159  await DCaReceiptRepository.BulkInsertAsync(listPhieuChi, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:4218  await DBaWithDrawRepository.BulkInsertAsync(listPhieuChi, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:4263  await DBADepositRepository.BulkInsertAsync(listPhieuChi, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_SoQuys\SoQuyManager.cs:4306  await DGLVoucherRepository.BulkInsertAsync(dglVouchers, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:276  await DBaWithDrawRepository.BulkInsertAsync(dBaWithDraws, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DBADeposits\DBADepositManager.cs:235  await DBADepositRepository.BulkInsertAsync(dBADeposits, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaPayments\DCaPaymentManager.cs:264  await DCaPaymentRepository.BulkInsertAsync(dCaPayments, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:272  await DCaReceiptRepository.BulkInsertAsync(dCaReceipts, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.CashBooks\DGLVouchers\EcomWallets\EcomWalletsAppService.cs:512  await _soQuyManager.DBADepositRepository.BulkInsertAsync(new List<DBADeposit> { autoDBaDeposit }, opt => opt.IncludeGraph = true);
src\TSD.POS.Customers\CrmSNhomKhachHangs\CrmSNhomKhachHangsAppService.cs:514  await _crmSNhomKhachHangsManager.CrmSNhomKhachHangRepository.BulkInsertAsync(new List<CrmSNhomKhachHang>() { CrmSNhomKhachHang }, opt => opt.IncludeGraph = true);
src\TSD.POS.Customers\CrmSNhomKhachHangs\CrmSNhomKhachHangsAppService.cs:547  await _crmSNhomKhachHangsManager.CrmSNhomKhachHangRepository.BulkInsertOrUpdateAsync(new List<CrmSNhomKhachHang>() { crmSNhomKhachHang }, opt => { opt.IncludeGraph = true; opt.PreserveInsertOrder = true; opt.SetOutputIde ...
src\TSD.POS.Customers\DKhachHangs\ImportExcel\ImportCustomerAppService.cs:189  await _dKhachHangManager.DKhachHangRepository.BulkInsertAsync(listDKhachHang, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; }).ConfigureAwait(false);
src\TSD.POS.Delivery\CrossCheck\CrossChecksAppService.cs:1038  await _crossCheckRepository.BulkInsertOrUpdateAsync(new List<CrossCheck> { crossCheck }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.ImportGoods\DatHangNhap\DDatHangNhapsAppService.cs:2389  await _dDatHangNhapRepository.BulkUpdateAsync(new List<DDatHangNhap> { savedDonDatHang }, opt => { opt.IncludeGraph = false; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:1461  await _dPhieuTraHangRepository.BulkUpdateAsync(new List<DPhieuTraHang> { savedPhieuTraHang }, opt => { opt.IncludeGraph = false; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2249  await _phieuTraHangChiTietRepository.BulkInsertOrUpdateAsync(phieuTraHangChiTietCapNhats, opt => { opt.SetOutputIdentity = true; opt.IncludeGraph = true; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2252  await _phieuTraHangChiTietRepository.BulkInsertAsync(phieuTraHangChiTietThemMois, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
```

### PreserveInsertOrder (20 hits)

```
src\TSD.POS.Application\SThongTinDangKys\Seed\MauInCreator.cs:41  await SMauInRepository.BulkInsertAsync(mauIns, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:206  opt.PreserveInsertOrder = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:396  opt.PreserveInsertOrder = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:612  opt.PreserveInsertOrder = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:2240  await _bulkAcenLoRepository.BulkInsertAsync(insertedAcenLos, opt => { opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\ThietLapSanPhams\ThietLapSanPhamManager.cs:110  await ThietLapSanPhamRepository.BulkInsertOrUpdateAsync(thietLapSanPhams, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_QuayBanHang\DDonHangs\CreateOrdersProcessManager.cs:175  opt.PreserveInsertOrder = true;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangManager.cs:2055  await DLichSuGiaoHangRepository.BulkInsertAsync(lichSuGiaoHangs, opt => { opt.PreserveInsertOrder = preserveInsertOrder; });
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:621  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dTraHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:276  await DBaWithDrawRepository.BulkInsertAsync(dBaWithDraws, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DBADeposits\DBADepositManager.cs:235  await DBADepositRepository.BulkInsertAsync(dBADeposits, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaPayments\DCaPaymentManager.cs:264  await DCaPaymentRepository.BulkInsertAsync(dCaPayments, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:272  await DCaReceiptRepository.BulkInsertAsync(dCaReceipts, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.Customers\CrmSNhomKhachHangs\CrmSNhomKhachHangsAppService.cs:547  await _crmSNhomKhachHangsManager.CrmSNhomKhachHangRepository.BulkInsertOrUpdateAsync(new List<CrmSNhomKhachHang>() { crmSNhomKhachHang }, opt => { opt.IncludeGraph = true; opt.PreserveInsertOrder = true; opt.SetOutputIde ...
src\TSD.POS.Delivery\CrossCheck\CrossChecksAppService.cs:1038  await _crossCheckRepository.BulkInsertOrUpdateAsync(new List<CrossCheck> { crossCheck }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.ImportGoods\DPhieuTraHangs\DPhieuTraHangsAppService.cs:2252  await _phieuTraHangChiTietRepository.BulkInsertAsync(phieuTraHangChiTietThemMois, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Sales\DDonHangs\DDonHangsWriteAppService.cs:217  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dDonHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Sales\DDonHangs\DDonHangsWriteAppService.cs:523  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(listDonHangs, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:630  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; });
```

### PropertiesToInclude (82 hits)

```
src\TSD.POS.Accountings\STaiKhoanNganHang\BankAccountManager.cs:797  await _sBranchBankAccountsRepository.BulkUpdateAsync(entities, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:237  }, opt => opt.PropertiesToInclude = new List<string> { nameof(ThongTinCuaHang.CKSType) });
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:248  }, opt => opt.PropertiesToInclude = new List<string> { nameof(SThongTinDangKy.CKSType) });
src\TSD.POS.Application\DDigitalSignatureInfos\DDigitalSignatureInfosAppService.cs:295  await _dDigitalSignatureInfoRepository.BulkUpdateAsync(dDigitalSignatureInfos, opt => opt.PropertiesToInclude = new List<string> { nameof(DDigitalSignatureInfo.IsDefault) });
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:1123  await _dHoaDonDienTuRepository.BulkUpdateAsync(batch, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:1322  }).ToList(), opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:403  opt.PropertiesToInclude = new List<string> { nameof(EcoProduct.ErrorMessage) };
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:540  await EcoProductRepository.BulkUpdateAsync(listEcoProducts, opt => opt.PropertiesToInclude = new List<string> { nameof(EcoProduct.UnitId), nameof(EcoProduct.ErrorMessage) });
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:673  opt.PropertiesToInclude = new List<string> { nameof(DHoaDonDienTu.EivId) };
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:1491  await EivMauSoKyHieuRepository.BulkUpdateAsync(eivTemplates, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:1548  await DHoaDonDienTuRepository.BulkUpdateAsync(einvoices, opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:2350  }).ToList(), opt => opt.PropertiesToInclude = new List<string> { nameof(DDonHang.EivSendingRequestStatus) });
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:2432  }).ToList(), opt => opt.PropertiesToInclude = new List<string> { nameof(DDonHang.TaxAuthorityCode), nameof(DDonHang.AdjustedTaxAuthorityCode) });
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:326  await _vNPTInvoiceRepository.BulkUpdateAsync(updatedInvoices, opt => opt.PropertiesToInclude = new List<string> { nameof(VNPTInvoice.TaxStatus) });
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:840  }).ToList(), opt => opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\PurchaseInvoices\PurchaseInvoicesManager.cs:395  opt.PropertiesToInclude = new List<string> { nameof(EivDPurchaseInvoiceMappingProduct.CrmSanPhamId), nameof(EivDPurchaseInvoiceMappingProduct.DonViTinhId) };
src\TSD.POS.Application\Staxes\StaxesAppService.cs:275  opt.PropertiesToInclude = new List<string> {
src\TSD.POS.Application\Staxes\StaxesManager.cs:191  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:233  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:275  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:317  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:353  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:388  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:423  opt.PropertiesToInclude = new List<string>
src\TSD.POS.Application\Staxes\StaxesManager.cs:466  opt.PropertiesToInclude = new List<string> { nameof(SThietLapHeThong.Value) };
src\TSD.POS.Application\ThongTinCuaHangs\ThongTinCuaHangsAppService.cs:307  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.TrangThai) };
src\TSD.POS.Application\ThongTinCuaHangs\ThongTinCuaHangsAppService.cs:342  opt.PropertiesToInclude = new List<string> { nameof(User.IsActive) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:341  opt.PropertiesToInclude = new List<string> { nameof(DKhachHang.IsAddressStandardized), nameof(DKhachHang.DiaChi), nameof(DKhachHang.ProvinceCode), nameof(DKhachHang.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:360  opt.PropertiesToInclude = new List<string> { nameof(CrmSNhaCungCap.IsAddressStandardized), nameof(CrmSNhaCungCap.DiaChi), nameof(CrmSNhaCungCap.ProvinceCode), nameof(CrmSNhaCungCap.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:385  opt.PropertiesToInclude = new List<string> { nameof(ThongTinCuaHang.IsAddressStandardized), nameof(ThongTinCuaHang.DiaChi), nameof(ThongTinCuaHang.ProvinceCode), nameof(ThongTinCuaHang.WardCode), nameof(ThongTinCuaHang.F ...
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:403  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.IsAddressStandardized), nameof(HaSCoCauToChuc.DiaChi), nameof(HaSCoCauToChuc.ProvinceCode), nameof(HaSCoCauToChuc.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:427  opt.PropertiesToInclude = new List<string> { nameof(AcenStock.IsAddressStandardized), nameof(AcenStock.PosAddress), nameof(AcenStock.ProvinceCode), nameof(AcenStock.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:448  opt.PropertiesToInclude = new List<string> { nameof(DNguoiNhanKhac.IsAddressStandardized), nameof(DNguoiNhanKhac.DiaChi), nameof(DNguoiNhanKhac.ProvinceCode), nameof(DNguoiNhanKhac.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:467  opt.PropertiesToInclude = new List<string> { nameof(DDoiTacGiaoHang.IsAddressStandardized), nameof(DDoiTacGiaoHang.DiaChi), nameof(DDoiTacGiaoHang.ProvinceCode), nameof(DDoiTacGiaoHang.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:484  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.IsAddressStandardized), nameof(HaSCoCauToChuc.DiaChi), nameof(HaSCoCauToChuc.ProvinceCode), nameof(HaSCoCauToChuc.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:502  opt.PropertiesToInclude = new List<string> { nameof(SThongTinDangKy.IsAddressStandardized), nameof(SThongTinDangKy.DiaChi), nameof(SThongTinDangKy.ProvinceCode), nameof(SThongTinDangKy.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:517  opt.PropertiesToInclude = new List<string> { nameof(AcenStock.IsAddressStandardized), nameof(AcenStock.PosAddress), nameof(AcenStock.ProvinceCode), nameof(AcenStock.WardCode) };
src\TSD.POS.Application\_NhapHang\NhaCungCap\CrmSNhaCungCaps\CrmSNhaCungCapsManager.cs:300  opt.PropertiesToInclude = new List<string> { nameof(CrmSNhaCungCap.MaSoThue), nameof(CrmSNhaCungCap.LastModificationTime) };
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:345  opt.PropertiesToInclude = new List<string> {
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:1140  }, opt => opt.PropertiesToInclude = new List<string> { nameof(SThietLapHeThong.Value) });
```

### SetOutputIdentity (140 hits)

```
src\TSD.POS.Accountings\Accounting\AccountingEntries\AccountingEntriesAppService.cs:456  await _accountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(accountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\CashBooks\CashBooksAccountingEntriesManager.cs:49  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\Accounting\AccountingEntries\ComboProcessings\ComboProcessingsAccountingEntriesManager.cs:102  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ConsumerGoodsIssues\ConsumerGoodsIssuesManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\DWorkShiftRecords\DWorkShiftRecordsEntriesManager.cs:54  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertedAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\DWorkShiftRecords\DWorkShiftRecordsEntriesManager.cs:95  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertedAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\GoodsDisposals\GoodsDisposalsManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\GoodsTransfers\GoodsTransfersEntriesManager.cs:53  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ImportGoods\ImportGoodsAccountingEntriesManager.cs:67  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\InventoryCounts\InventoryCountsEntriesManager.cs:43  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\OrdersAccountingEntriesManager.cs:151  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\PurchaseOrdersAccountingEntriesManager.cs:53  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\OrderVouchers\OrderVouchersEntriesManager.cs:100  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\PreImportGoods\PreImportGoodsAccountingEntriesManager.cs:48  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Accounting\AccountingEntries\ReturnPurchaseGoods\ReturnPurchaseGoodsAccountingEntriesManager.cs:60  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Authorization\HAAuthentication\HAApiManager.cs:118  await _dUserCentralRepository.BulkInsertAsync(listDUserCentral, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\Ecommerce\DEcoOrderCrossChecks\DEcoOrderCrossChecksManager.cs:428  await _dEcoOrderCrossCheckRepository.BulkInsertOrUpdateAsync(ecoOrderCrossChecks, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2039  await EcoOrderRepository.BulkInsertAsync(ecoOrders.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2045  await EcoOrderReturnRepository.BulkInsertAsync(ecoOrderReturns.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(f ...
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:157  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:225  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\EcoProductWorkerManager.cs:282  await _ecoProductRepository.BulkInsertOrUpdateAsync(listProduct, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:547  await EcoProductRepository.BulkInsertAsync(ecoProducts.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\SThongTinDangKys\Seed\MauInCreator.cs:41  await SMauInRepository.BulkInsertAsync(mauIns, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\Warehouses\InventoryCountsManager.cs:352  await TSanPhamKhoLoHanRepo.BulkInsertAsync(newTProductBathStocks, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Warehouses\InventoryCountsManager.cs:865  await _acenLoRepo.BulkInsertAsync(newBatchs, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\Warehouses\InventoryCountsManager.cs:930  await _crmSanPhamManager.SanPhamKhoLoHanRepository.BulkInsertAsync(productBatchStocks, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\_Common\File\FileManager.cs:720  await _binaryObjectRepository.BulkInsertAsync(httpResponse?.Data?.Result.BinaryObjects, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\_Common\File\FileManager.cs:882  await _binaryObjectRepository.BulkInsertAsync(binaryFiles, opt => { opt.SetOutputIdentity = true; });
src\TSD.POS.Application\_Kho\DTheKhos\DTheKhoManager.cs:143  await DTheKhoRepository.BulkInsertAsync(inventoryCards, opt => { opt.SetOutputIdentity = isCreateByPOS; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_Kho\DTheKhos\DTheKhoManager.cs:252  await DTheKhoRepository.BulkInsertAsync(inventoryCardBatch.Value.Where(o => o.Id == 0).ToList(), opt => { opt.WithHoldlock = false; opt.SetOutputIdentity = true; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:205  opt.SetOutputIdentity = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:395  opt.SetOutputIdentity = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:611  opt.SetOutputIdentity = true;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:885  await CrmSanPhamManager.HaSDonViTinhRepository.BulkInsertAsync(insertedUnits, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:2203  await _bulkCrmSerialRepository.BulkInsertAsync(insertedSerials, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:2240  await _bulkAcenLoRepository.BulkInsertAsync(insertedAcenLos, opt => { opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:76  opt.SetOutputIdentity = true;
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:84  await PosSanPhamRepository.BulkInsertOrUpdateAsync(posSanPhams.SelectMany(x => x.PosSanPhams).ToList(), opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:223  opt.SetOutputIdentity = true;
```

### UseTempDB (1 hits)

```
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:398  opt.UseTempDB = true;
```

### WithHoldlock (79 hits)

```
src\TSD.POS.Application\Accounting\AccountingEntries\CashBooks\CashBooksAccountingEntriesManager.cs:49  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\Accounting\AccountingEntries\Orders\OrdersAccountingEntriesManager.cs:151  await AccountingEntryValidator.AccountingEntryRepository.BulkInsertAsync(insertAccountingEntries, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\BaoCaos\BaoCaoBanHang\OrderReportManager.cs:314  await _donHangSanPhamRepository.BulkInsertAsync(listDonHangSanPhams, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:695  await _eivMauSoKyHieuRepository.BulkInsertAsync(insertedEivMauSoKyHieus, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\DHoaDonDienTus\DHoaDonDienTusAppService.cs:724  await _eivSHinhThucThanhToansRepository.BulkInsertAsync(insertEivSHinhThucThanhToans, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2039  await EcoOrderRepository.BulkInsertAsync(ecoOrders.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\Ecommerce\Orders\EcoOrdersWorkerManager.cs:2045  await EcoOrderReturnRepository.BulkInsertAsync(ecoOrderReturns.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(f ...
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:258  await EcoProductRepository.BulkInsertAsync(listProduct.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Products\EcoProductManager.cs:398  await EcoProductRepository.BulkInsertAsync(listProduct.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\Ecommerce\Products\SyncEcoProductManager.cs:547  await EcoProductRepository.BulkInsertAsync(ecoProducts.Where(x => x.Id <= 0).ToList(), opt => { opt.SetOutputIdentity = true; opt.BatchSize = TsdConst.BatchSize_1000; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:520  await EivCauHinhTruongMoRongRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:584  await EivMauSoKyHieuRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:636  await EivSHinhThucThanhToanRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:727  await _eivSDVTRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivDongBosManager.cs:777  await EivSCauHinhLPhiRepository.BulkInsertAsync(insertNewItems, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1162  if (eivOrderDetailAdds.Count > 0) await _eivOrderDetailRepository.BulkInsertAsync(eivOrderDetailAdds, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1179  await _dHoaDonDienTuRepository.BulkInsertAsync(items.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\HoaDonDienTu\EivManager.cs:1489  await _eivOrderErrorRepository.BulkInsertAsync(eivOrderErrorAddOrUpdates.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:518  await EivOrderDetailRepository.BulkInsertAsync(insertedEivOrderDetails, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:1129  await _eivOrderErrorRepository.BulkInsertAsync(eivOrderErrorAddOrUpdates.Where(x => x.Id <= 0).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\InvoiceConnection\VNPTEInvoice\Manager\VNPTInvoiceManager.cs:1218  await _vNPTInvoiceRepository.BulkInsertAsync(vNPTInvoices, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\_Kho\DTheKhos\DTheKhoManager.cs:143  await DTheKhoRepository.BulkInsertAsync(inventoryCards, opt => { opt.SetOutputIdentity = isCreateByPOS; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_Kho\DTheKhos\DTheKhoManager.cs:252  await DTheKhoRepository.BulkInsertAsync(inventoryCardBatch.Value.Where(o => o.Id == 0).ToList(), opt => { opt.WithHoldlock = false; opt.SetOutputIdentity = true; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:207  opt.WithHoldlock = false;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:373  await _bulkSanPhamCoreRepository.BulkInsertAsync(coreProducts, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:397  opt.WithHoldlock = false;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:590  await _bulkSanPhamCoreRepository.BulkInsertAsync(coreProducts, opt => opt.WithHoldlock = false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:613  opt.WithHoldlock = false;
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:885  await CrmSanPhamManager.HaSDonViTinhRepository.BulkInsertAsync(insertedUnits, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:2203  await _bulkCrmSerialRepository.BulkInsertAsync(insertedSerials, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\CrmSanPhamCreateManager.cs:2240  await _bulkAcenLoRepository.BulkInsertAsync(insertedAcenLos, opt => { opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:77  opt.WithHoldlock = false;
src\TSD.POS.Application\_NhapHang\SanPham\PosSanPhams\PosSanPhamsManager.cs:230  opt.WithHoldlock = false;
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:985  await _sThietLapHeThongRepository.BulkInsertAsync(systemConfigs, opts => { opts.SetOutputIdentity = true; opts.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:1248  await _sThietLapHeThongRepository.BulkInsertAsync(systemConfigs, opts => { opts.SetOutputIdentity = true; opts.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_QuanTriNguoiDung\ThietLapNghiepVu\SThietLapHeThongs\SThietLapHeThongsAppService.cs:1320  await _sThietLapHeThongRepository.BulkInsertAsync(systemConfigs, opts => { opts.SetOutputIdentity = true; opts.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Application\_QuanTriNguoiDung\ThongTinChung\SPaymentTerms\SPaymentTermsAppService.cs:195  }).ToList(), opt => opt.WithHoldlock = false);
src\TSD.POS.Application\_QuayBanHang\DDonHangs\CreateOrdersProcessManager.cs:75  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.WithHoldlock = false; });
src\TSD.POS.Application\_QuayBanHang\DDonHangs\CreateOrdersProcessManager.cs:176  opt.WithHoldlock = false;
src\TSD.POS.Application\_QuayBanHang\DDonHangs\DDonHangSyncManager.cs:1297  await _dDonHangRepository.BulkInsertAsync(posPreOrderInputBatch.Value, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
```

## BulkConfig instantiations (samples)

### BulkConfig (59 hits)

```
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:105  public async Task BulkUpdateAsync(List<DVanDon> vanDons, Action<BulkConfig> bulkAction = null)
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:118  public async Task BulkInsertAsync(List<DVanDon> vanDons, Action<BulkConfig> bulkAction)
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:89  public async Task Inserts(List<DObjectLedgerDto> listObjectLedgerDto, bool isUpdateCongNo = true, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:107  public async Task<List<DObjectLedger>> Inserts(List<DObjectLedger> listObjectLedger, DoiTuongThuChi? loaiDoiTuong = null, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:155  public async Task Deletes(List<DObjectLedger> listObjectLedger, DoiTuongThuChi? loaiDoiTuong = null, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:450  private async Task BulkInsertAsync(List<DObjectLedger> listObjectLedger, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:452  Action<BulkConfig> defaultConfig = (opt) => opt.WithHoldlock = false;
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:72  public async Task BulkInsertAsync(IList<DBaWithDraw> dBaWithDraws, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:79  public async Task BulkInsertAsync(IList<DCaReceipt> dCaReceipts, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:43  public async Task BulkInsertAsync(IList<DGLVoucherCrossEntryDetail> vouchers, Action<BulkConfig> config = null)
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:54  public async Task BulkDeleteAsync(IList<DGLVoucherCrossEntryDetail> vouchers, Action<BulkConfig> config = null)
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:14  void BulkDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:15  void BulkDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:16  Task BulkDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:17  Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:18  void BulkInsert(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:19  void BulkInsert(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:20  Task BulkInsertAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:21  Task BulkInsertAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:22  void BulkInsertOrUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:23  void BulkInsertOrUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:24  Task BulkInsertOrUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:25  Task BulkInsertOrUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:26  void BulkInsertOrUpdateOrDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:27  void BulkInsertOrUpdateOrDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:28  Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:29  Task BulkInsertOrUpdateOrDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:30  void BulkRead(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:31  void BulkRead(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:32  Task BulkReadAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:33  Task BulkReadAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:34  void BulkUpdate(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:35  void BulkUpdate(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:36  Task BulkUpdateAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:37  Task BulkUpdateAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:51  public void BulkDelete(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:56  public void BulkDelete(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null)
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:61  public async Task BulkDeleteAsync(IList<TEntity> entities, Action<BulkConfig> bulkAction, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:66  public async Task BulkDeleteAsync(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null, CancellationToken cancellationToken = default)
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\Repositories\POSBulkRepository.cs:71  public void BulkInsert(IList<TEntity> entities, BulkConfig bulkConfig = null, Action<decimal> progress = null, Type type = null)
```

## EF model features (samples)

### DefaultValue (1 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContext.cs:1893  s.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
```

### GlobalQueryFilter (1 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContext.cs:3932  b.HasQueryFilter(m => !m.IsDeleted)
```

### SoftDelete_ABP (408 hits)

```
src\TSD.POS.Application.Shared\_QuanTriNguoiDung\ThongTinChung\CoCauToChuc\HaSCoCauToChucs\Dtos\CreateOrEditHaSCoCauToChucDto.cs:11  public class CreateOrEditHaSCoCauToChucDto : EntityDto<int?>, IMayHaveTenant
src\TSD.POS.Core\Accounting\AccountingEntries\AccountingEntry.cs:15  public class AccountingEntry : AuditedEntity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\AcenSyncHistories\AcenSyncHistory.cs:11  public class AcenSyncHistory : CreationAuditedEntity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\AcenSyncHistories\AcenSyncHistoryDetail.cs:10  public class AcenSyncHistoryDetail : Entity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\WorkShiftRecord\DWorkShiftRecord.cs:14  public class DWorkShiftRecord : FullAuditedEntity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\WorkShiftRecord\WorkShiftRecordEmployee.cs:9  public class WorkShiftRecordEmployee : Entity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\WorkShiftRecord\WorkShiftRecordMoney.cs:11  public class WorkShiftRecordMoney : Entity<long>, IMayHaveTenant
src\TSD.POS.Core\Accounting\WorkShiftRecord\WorkShiftRecordMoneyDetail.cs:11  public class WorkShiftRecordMoneyDetail : Entity<long>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenAsPaymentTerms\AcenAsPaymentTerm.cs:10  public class AcenAsPaymentTerm : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenAutoBusiness\AcenAutoBusinessEntity.cs:12  public class AcenAutoBusinessEntity : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenAutoBusinessEntityPosExs\AcenAutoBusinessEntityPosEx.cs:15  public class AcenAutoBusinessEntityPosEx : AuditedEntity<long>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSAccount\AcenSAccount.cs:10  public class AcenSAccount : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSAccountDefaults\AcenSAccountDefault.cs:11  public class AcenSAccountDefault : FullAuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSAccountTransfers\AcenSAccountTransfer.cs:11  public class AcenSAccountTransfer : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSBankAccountOrganizations\AcenSBankAccountOrganization.cs:16  public class AcenSBankAccountOrganization : AuditedEntity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid
src\TSD.POS.Core\AcenDB\Dictionary\AcenSBankAccountOrganizations\AcenSBankAccountOrganizationViews.cs:15  public class AcenSBankAccountOrganizationViews : Entity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid
src\TSD.POS.Core\AcenDB\Dictionary\AcenSBankAccounts\AcenSBankAccount.cs:14  public class AcenSBankAccount : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSBanks\AcenSBank.cs:11  public class AcenSBank : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSBudgetItems\AcenSBudgetItem.cs:10  public class AcenSBudgetItem : CreationAuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSCostElementOrganizations\AcenSCostElementOrganization.cs:13  public class AcenSCostElementOrganization : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSCostElements\AcenSCostElement.cs:12  public class AcenSCostElement : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSCostElements\AcenSCostElementProduct.cs:11  public class AcenSCostElementProduct : Entity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSExpenseItems\AcenSExpenseItem.cs:13  public class AcenSExpenseItem : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSExpenseItems\AcenSExpenseItemBranch.cs:9  public class AcenSExpenseItemBranch : Entity<long>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSListItems\AcenSListItem.cs:11  public class AcenSListItem : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSOrganizations\AcenSOrganizationEntity.cs:11  public class AcenSOrganizationEntity : CreationAuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSPaymentMethodTypes\AcenSPaymentMethodTypes.cs:10  public class AcenSPaymentMethodTypes : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSProjectWorkCategories\AcenSProjectWorkCategory.cs:11  public class AcenSProjectWorkCategory : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSProjectWorks\AcenSProjectWork.cs:13  public class AcenSProjectWork : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSProjectWorks\AcenSProjectWorkOrganizations.cs:12  public class AcenSProjectWorkOrganizations : AuditedEntity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid
src\TSD.POS.Core\AcenDB\Dictionary\AcenSPurchasePurposes\AcenSPurchasePurpose.cs:9  public class AcenSPurchasePurpose : AuditedEntity<Guid>//, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Dictionary\AcenSUnits\AcenSUnit.cs:10  public class AcenSUnit : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Voucher\AcenDGeneralLedgers\AcenDGeneralLedgers.cs:18  public class AcenDGeneralLedgers : AuditedEntity<Guid>, IMayHaveTenant, IMayHaveDisplayOnBook, IMayHaveOrganizationGuid
src\TSD.POS.Core\AcenDB\Voucher\Contract\AcenDPUContractDetails\AcenDPUContractDetail.cs:14  public class AcenDPUContractDetail : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Voucher\Contract\AcenDPUContracts\AcenDPUContract.cs:16  public class AcenDPUContract : FullAuditedEntity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid
src\TSD.POS.Core\AcenDB\Voucher\Inventories\AcenDInInwardDetails\AcenDInInwardDetail.cs:19  public class AcenDInInwardDetail : AuditedEntity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Voucher\Inventories\AcenDInInwards\AcenDInInward.cs:16  public class AcenDInInward : FullAuditedEntity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid, IMayHaveDisplayOnBook
src\TSD.POS.Core\AcenDB\Voucher\Inventories\AcenDInOutwardDetails\AcenDInOutwardDetail.cs:15  public class AcenDInOutwardDetail : Entity<Guid>, IMayHaveTenant
src\TSD.POS.Core\AcenDB\Voucher\Inventories\AcenDInOutwards\AcenDInOutward.cs:17  public class AcenDInOutward : FullAuditedEntity<Guid>, IMayHaveTenant, IMayHaveOrganizationGuid, IMayHaveDisplayOnBook
src\TSD.POS.Core\AcenDB\Voucher\OpeningAccountEntry\AcenDOpeningAccountEntry.cs:13  public class AcenDOpeningAccountEntry : CreationAuditedEntity<long>, IMayHaveTenant
```

## DB providers (samples)

### SqlServer (2 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContextConfigurer.cs:10  builder.UseSqlServer(connectionString);
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContextConfigurer.cs:15  builder.UseSqlServer(connection);
```


