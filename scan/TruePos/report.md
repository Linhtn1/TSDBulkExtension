# EFCore.BulkExtensions usage scan

Generated: 2026-09-11T10:48:31  
Roots: D:\LinhTN\Projects\TruePos\src  
Files: 9077 .cs, 62 .csproj/.props

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
| BatchSize | 1 | 1 |
| IncludeGraph | 8 | 5 |
| PreserveInsertOrder | 5 | 4 |
| PropertiesToInclude | 8 | 4 |
| SetOutputIdentity | 12 | 6 |
| WithHoldlock | 9 | 3 |

## EF model features affecting bulk mapping

| Name | Hits | Files |
|---|---|---|
| ShadowProperty | 1150027 | 170 |
| RowVersion | 340 | 170 |
| DefaultValue | 130 | 130 |
| Discriminator_TPH | 2906 | 170 |
| GlobalQueryFilter | 1 | 1 |
| Identity_HiLo_Seq | 104555 | 170 |
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

### BatchSize (1 hits)

```
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:493  opt.BatchSize = TsdConst.BatchSize_1000;
```

### IncludeGraph (8 hits)

```
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:621  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dTraHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:1382  await _dDonHangRepository.BulkInsertAsync(returnGoodsOrders, opt => { opt.IncludeGraph = true; });
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:276  await DBaWithDrawRepository.BulkInsertAsync(dBaWithDraws, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:272  await DCaReceiptRepository.BulkInsertAsync(dCaReceipts, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:372  await DbContextBulkExtensions.BulkInsertAsync(dbContext, newCrmSanPhamDonViTinhs, opt => opt.IncludeGraph = true).ConfigureAwait(false);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:630  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; });
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:839  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
```

### PreserveInsertOrder (5 hits)

```
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:621  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dTraHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:276  await DBaWithDrawRepository.BulkInsertAsync(dBaWithDraws, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:272  await DCaReceiptRepository.BulkInsertAsync(dCaReceipts, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:630  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; });
```

### PropertiesToInclude (8 hits)

```
src\TSD.POS.Application\ThongTinCuaHangs\ThongTinCuaHangsAppService.cs:307  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.TrangThai) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:403  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.IsAddressStandardized), nameof(HaSCoCauToChuc.DiaChi), nameof(HaSCoCauToChuc.ProvinceCode), nameof(HaSCoCauToChuc.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:448  opt.PropertiesToInclude = new List<string> { nameof(DNguoiNhanKhac.IsAddressStandardized), nameof(DNguoiNhanKhac.DiaChi), nameof(DNguoiNhanKhac.ProvinceCode), nameof(DNguoiNhanKhac.WardCode) };
src\TSD.POS.Application\_Common\CommonLookupAppService.cs:484  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.IsAddressStandardized), nameof(HaSCoCauToChuc.DiaChi), nameof(HaSCoCauToChuc.ProvinceCode), nameof(HaSCoCauToChuc.WardCode) };
src\TSD.POS.Application\_QuanTriNguoiDung\ThongTinChung\CoCauToChuc\HaSCoCauToChucs\HaSCoCauToChucsAppService.cs:1651  opt.PropertiesToInclude = new List<string> { nameof(HaSCoCauToChuc.ItemOrder) };
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:492  opt.PropertiesToInclude = new List<string> { nameof(DDonHang.EcoShopId) };
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:704  }, opt => opt.PropertiesToInclude = new List<string> { nameof(PromotionProcessingDetail.OrderId) });
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:2006  await _crmSanPhamRepository.BulkUpdateAsync(productDrafts, opt => opt.PropertiesToInclude = new List<string> { nameof(CrmSanPham.IsDraft) });
```

### SetOutputIdentity (12 hits)

```
src\TSD.POS.Application\_QuayBanHang\DVanDons\DVanDonsManager.cs:621  await _dDonHangRepository.BulkInsertAsync(new List<DDonHang>() { dTraHang }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\BAWithDraws\BAWithDrawManager.cs:276  await DBaWithDrawRepository.BulkInsertAsync(dBaWithDraws, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; });
src\TSD.POS.Application\_SoQuys\DCaReceipts\DCaReceiptManager.cs:272  await DCaReceiptRepository.BulkInsertAsync(dCaReceipts, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; }).ConfigureAwait(false);
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:176  await DGLVoucherCrossEntryDetailRepository.BulkInsertOrUpdateAsync(output, opt => opt.SetOutputIdentity = true);
src\TSD.POS.Application\_SoQuys\VoucherCrossEntry\DGLVoucherCrossEntryDetailManager.cs:207  await DGLVoucherCrossEntryDetailRepository.BulkInsertOrUpdateAsync(output, opt => opt.SetOutputIdentity = true).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:605  await _crmNhomSanPhamsManager.ProductGroupRepository.BulkInsertAsync(insertProductGroups, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:630  await _donViTinhRepository.BulkInsertAsync(insertedUnits, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:654  await _sThuongHieuRepository.BulkInsertAsync(insertThuongHieus, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:682  await _haSThuocTinhRepository.BulkInsertAsync(insertProperties, opt => { opt.SetOutputIdentity = true; }).ConfigureAwait(false);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:630  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; });
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:839  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; });
```

### WithHoldlock (9 hits)

```
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:452  Action<BulkConfig> defaultConfig = (opt) => opt.WithHoldlock = false;
src\TSD.POS.Application\_SoQuys\DObjectLedgerManager.cs:454  defaultConfig = (opt) => { opt.WithHoldlock = false; config(opt); };
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:261  await _sanPhamCoreRepository.BulkInsertAsync(newSanPhamCores, opt => opt.WithHoldlock = false).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:285  await _crmSanPhamRepository.BulkInsertAsync(newCrmSanPhams, opt => opt.WithHoldlock = false).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:605  await _crmNhomSanPhamsManager.ProductGroupRepository.BulkInsertAsync(insertProductGroups, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:630  await _donViTinhRepository.BulkInsertAsync(insertedUnits, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Products\CrmSanPhams\Importing\ImportCrmSanPhamAppService.cs:654  await _sThuongHieuRepository.BulkInsertAsync(insertThuongHieus, opt => { opt.SetOutputIdentity = true; opt.WithHoldlock = false; }).ConfigureAwait(false);
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:590  await _dDonHangManager.DDonHangRepository.BulkInsertOrUpdateAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false;  ...
src\TSD.POS.Sales\DDonHangs\OrdersAppService.cs:630  await _dDonHangManager.DDonHangRepository.BulkInsertAsync(new List<DDonHang>() { order }, opt => { opt.IncludeGraph = true; opt.SetOutputIdentity = true; opt.PreserveInsertOrder = true; opt.WithHoldlock = false; });
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

### ShadowProperty (1150027 hits)

```
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:27  b.Property<int>("Id")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:31  b.Property<DateTime>("CreationTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:34  b.Property<long?>("CreatorUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:37  b.Property<long?>("DeleterUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:40  b.Property<DateTime?>("DeletionTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:43  b.Property<string>("DisplayName")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:48  b.Property<bool>("IsDeleted")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:51  b.Property<DateTime?>("LastModificationTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:54  b.Property<long?>("LastModifierUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:57  b.Property<string>("Name")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:69  b.Property<long>("Id")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:73  b.Property<DateTime>("CreationTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:76  b.Property<long?>("CreatorUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:79  b.Property<string>("Discriminator")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:83  b.Property<string>("Name")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:88  b.Property<string>("Value")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:102  b.Property<long>("Id")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:106  b.Property<string>("BrowserInfo")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:110  b.Property<string>("ClientIpAddress")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:114  b.Property<string>("ClientName")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:118  b.Property<string>("CustomData")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:122  b.Property<string>("Exception")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:126  b.Property<int>("ExecutionDuration")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:129  b.Property<DateTime>("ExecutionTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:132  b.Property<int?>("ImpersonatorTenantId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:135  b.Property<long?>("ImpersonatorUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:138  b.Property<string>("MethodName")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:142  b.Property<string>("Parameters")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:146  b.Property<string>("ServiceName")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:150  b.Property<int?>("TenantId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:153  b.Property<long?>("UserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:169  b.Property<long>("Id")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:173  b.Property<DateTime>("CreationTime")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:176  b.Property<long?>("CreatorUserId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:179  b.Property<string>("Discriminator")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:183  b.Property<bool>("IsGranted")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:186  b.Property<string>("Name")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:191  b.Property<int?>("TenantId")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:205  b.Property<long>("Id")
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:209  b.Property<string>("ClaimType")
```

### RowVersion (340 hits)

```
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:928  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1003  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:971  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1046  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:971  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1046  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:971  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1046  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:974  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1049  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170913133916_Added_SharedMessageId_To_ChatMessage.Designer.cs:979  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170913133916_Added_SharedMessageId_To_ChatMessage.Designer.cs:1054  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914070123_Added_ReceiverReadState_To_ChatMessage.Designer.cs:979  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914070123_Added_ReceiverReadState_To_ChatMessage.Designer.cs:1054  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914084815_Invoice_Changes.Designer.cs:974  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914084815_Invoice_Changes.Designer.cs:1049  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914121022_TypeChange_SharedMessageId_String_Guid.Designer.cs:979  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20170914121022_TypeChange_SharedMessageId_String_Guid.Designer.cs:1054  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180118065703_Added_Entity_History.Designer.cs:1108  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180118065703_Added_Entity_History.Designer.cs:1183  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180320065710_Upgraded_To_Abp_V3_5.Designer.cs:1111  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180320065710_Upgraded_To_Abp_V3_5.Designer.cs:1187  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180718081610_Abp_v3_7_Changes.Designer.cs:1124  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180718081610_Abp_v3_7_Changes.Designer.cs:1201  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180726063233_Upgraded_ABP_v3.8.0.Designer.cs:1124  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180726063233_Upgraded_ABP_v3.8.0.Designer.cs:1201  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180731052649_Upgrade_ABP_v3.8.1.Designer.cs:1124  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180731052649_Upgrade_ABP_v3.8.1.Designer.cs:1201  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180807062930_Upgrade_ABP_v3.8.2.Designer.cs:1127  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20180807062930_Upgrade_ABP_v3.8.2.Designer.cs:1204  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20181012141151_Upgraded_To_Abp_v3_9_0.Designer.cs:1127  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20181012141151_Upgraded_To_Abp_v3_9_0.Designer.cs:1204  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190103081952_Recurring_Payment_Changes.Designer.cs:1127  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190103081952_Recurring_Payment_Changes.Designer.cs:1204  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190208083524_Upgraded_To_Abp_v4_2_0.Designer.cs:1161  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190208083524_Upgraded_To_Abp_v4_2_0.Designer.cs:1238  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190304131651_Added_User_OrganizationUnits.Designer.cs:1163  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190304131651_Added_User_OrganizationUnits.Designer.cs:1240  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190501074213_Changed_Billing_Setting_Names.Designer.cs:1163  .IsConcurrencyToken()
src\TSD.POS.EntityFrameworkCore\Migrations\20190501074213_Changed_Billing_Setting_Names.Designer.cs:1240  .IsConcurrencyToken()
```

### DefaultValue (130 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContext.cs:1893  s.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20250601084158_Initial_POS.Designer.cs:29552  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20250601084522_Initial_POS_MigrationScripts.Designer.cs:29552  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20250825081009_Added_ASReasonsTable.Designer.cs:29845  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20250826015913_Updated_OriginTypeForASReasonTable.Designer.cs:29845  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20250826092510_Added_DReasonFeaturesTable.Designer.cs:29845  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251003094616_Add_Column_Link_EcoProduct.Designer.cs:29574  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251015041350_UpdateTypeOfInvnetoryCountingDetail.Designer.cs:29574  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251015095527_UpdateTypeForBathSerialInventoryCounting.Designer.cs:29574  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251023095329_UpdateStoreInfoExpiredTime.Designer.cs:29574  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251024084351_UpdateTypeActuallyQuantity.Designer.cs:29574  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251027072739_ADD_VNPTInvoices_Table.Designer.cs:29667  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251028074154_SetNullableForsummaryColumns.Designer.cs:29667  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251030015209_AlterInventoryCountingDetail.Designer.cs:29667  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251106035637_add_table_DActionHistories_DApprovalHistories.Designer.cs:29753  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251107021707_add_col_ApprovalType_to_DGeneralGoodsIssues_DDonHangs_SoQuyBaseEntity_DPhieuNhapKhos_DPhieuTraHangs.Designer.cs:29783  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251108030150_UpdateSLyDo_AddMoreType.Designer.cs:29789  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251113023114_AddColumn_BinCode_SNganHang.Designer.cs:29793  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251114085137_Add_SPaymentTerms_Table.Designer.cs:29900  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251117091141_Add_OrderId_DGeneralGoodsIssue.Designer.cs:29903  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251118094917_Update-DebtRefNo.Designer.cs:29903  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251120013551_Edit_OutBoundEnum_OrderDetail.Designer.cs:29903  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251120042028_add-OutboundType-to-GeneralGoodsIssueDetail.Designer.cs:29906  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251120121543_Add_PromotionProcessing_Table.Designer.cs:29906  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251122065433_Add_col_CKSType_and_RefIDLevel2.Designer.cs:29910  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251124122121_add_table_for_feature_HoaDonMuaVao.Designer.cs:30184  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251126084404_UpdateTable_RemoveNameField_PromotionProcessing.Designer.cs:30184  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251127041407_UpdateEivDPurchaseInvoice.Designer.cs:30192  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251127072033_Add_IsApplyAll_PromotionProcessing.Designer.cs:30196  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251127080012_Additional-ResponseInvoice.Designer.cs:30202  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251203070631_Add_InventoryNormTableGroup.Designer.cs:30505  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251203073456_Add_DProductionOrderDetails_Table.Designer.cs:30605  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251204080418_Add_DInInward_Tables.Designer.cs:30807  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251204101931_AddDKPICampaignTable.Designer.cs:30885  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251204112248_AddDKPICampaignDetailTable_20251204.Designer.cs:30951  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251204114138_DanhIndexChoCacBangChiTietChienDichKPI.Designer.cs:30955  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251204123509_update_DB_Nhapxuatkho.Designer.cs:30629  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251205022221_add_Table_Chuyenkho_productionImport.Designer.cs:30741  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251205023647_update_DInInWard_KienNT.Designer.cs:31094  .HasDefaultValueSql("newsequentialid()");
src\TSD.POS.EntityFrameworkCore\Migrations\20251205040553_add_PK _NhapXuatKho_KienNT.Designer.cs:31094  .HasDefaultValueSql("newsequentialid()");
```

### Discriminator_TPH (2906 hits)

```
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:97  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:200  b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1317  b.HasDiscriminator().HasValue("EditionFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1331  b.HasDiscriminator().HasValue("TenantFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1345  b.HasDiscriminator().HasValue("RolePermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1359  b.HasDiscriminator().HasValue("UserPermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:71  b.HasDiscriminator<string>("Discriminator").HasValue("Edition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:104  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:207  b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1446  b.HasDiscriminator().HasValue("SubscribableEdition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1460  b.HasDiscriminator().HasValue("EditionFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1474  b.HasDiscriminator().HasValue("TenantFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1488  b.HasDiscriminator().HasValue("RolePermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:1502  b.HasDiscriminator().HasValue("UserPermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:71  b.HasDiscriminator<string>("Discriminator").HasValue("Edition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:104  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:207  b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1449  b.HasDiscriminator().HasValue("SubscribableEdition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1463  b.HasDiscriminator().HasValue("EditionFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1477  b.HasDiscriminator().HasValue("TenantFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1491  b.HasDiscriminator().HasValue("RolePermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170704084731_Added_GoogleAuthenticatorKey_Column.Designer.cs:1505  b.HasDiscriminator().HasValue("UserPermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:71  b.HasDiscriminator<string>("Discriminator").HasValue("Edition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:104  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:207  b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1451  b.HasDiscriminator().HasValue("SubscribableEdition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1465  b.HasDiscriminator().HasValue("EditionFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1479  b.HasDiscriminator().HasValue("TenantFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1493  b.HasDiscriminator().HasValue("RolePermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170714081027_Added_Relation_Between_Edition_And_SubscriptionPayment.Designer.cs:1507  b.HasDiscriminator().HasValue("UserPermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:71  b.HasDiscriminator<string>("Discriminator").HasValue("Edition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:104  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:207  b.HasDiscriminator<string>("Discriminator").HasValue("PermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1454  b.HasDiscriminator().HasValue("SubscribableEdition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1468  b.HasDiscriminator().HasValue("EditionFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1482  b.HasDiscriminator().HasValue("TenantFeatureSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1496  b.HasDiscriminator().HasValue("RolePermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170724142223_Upgraded_To_Abp_V2_2.Designer.cs:1510  b.HasDiscriminator().HasValue("UserPermissionSetting");
src\TSD.POS.EntityFrameworkCore\Migrations\20170913133916_Added_SharedMessageId_To_ChatMessage.Designer.cs:76  b.HasDiscriminator<string>("Discriminator").HasValue("Edition");
src\TSD.POS.EntityFrameworkCore\Migrations\20170913133916_Added_SharedMessageId_To_ChatMessage.Designer.cs:109  b.HasDiscriminator<string>("Discriminator").HasValue("FeatureSetting");
```

### GlobalQueryFilter (1 hits)

```
src\TSD.POS.EntityFrameworkCore\EntityFrameworkCore\POSDbContext.cs:3932  b.HasQueryFilter(m => !m.IsDeleted)
```

### Identity_HiLo_Seq (104555 hits)

```
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:28  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:70  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:103  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:170  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:206  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:244  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:304  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:337  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:370  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:417  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:447  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:479  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:509  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:553  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:595  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:646  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:694  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:751  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:794  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:844  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:872  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:924  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:992  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1124  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1169  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1217  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170406083347_Initial_Migration.Designer.cs:1289  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:29  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:77  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:110  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:177  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:213  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:246  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:306  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:339  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:372  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:419  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:449  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:481  .ValueGeneratedOnAdd()
src\TSD.POS.EntityFrameworkCore\Migrations\20170623075109_AspNetZero_V4_1_Changes.Designer.cs:511  .ValueGeneratedOnAdd()
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


