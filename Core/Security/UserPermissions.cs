using System;

namespace Core.Security
{
    public static class UserPermissions
    {
        public static void SetUserType(UserType userType)
        {
            CanManageCustomers = false;
            CanManageInventory = false;
            CanManageSuppliers = false;
            CanManageEmployees = false;
            CanPlaceOrder = false;
            CanPlaceSupplyOrder = false;
            CanManageReturns = false;
            NotifyDueCreditBills = false;
            CanSettleCreditBillsAndViewDetails = false;
            NotifyLowStocks = false;
            CanGenerateTransactionReport = false;
            CanViewActivityLog = false;

            switch (userType)
            {
                case UserType.Manager:
                    CanManageCustomers = true;
                    CanManageInventory = true;
                    CanManageSuppliers = true;
                    CanSettleCreditBillsAndViewDetails = true;
                    CanManageEmployees = true;
                    CanPlaceOrder = true;
                    CanManageReturns = true;
                    CanPlaceSupplyOrder = true;
                    NotifyDueCreditBills = true;
                    NotifyLowStocks = true;
                    CanGenerateTransactionReport = true;
                    CanViewActivityLog = true;
                    break;
                case UserType.InventoryManager:
                    CanManageReturns = true;
                    CanPlaceSupplyOrder = true;
                    NotifyLowStocks = true;
                    break;
                case UserType.Cashier:
                    CanSettleCreditBillsAndViewDetails = true;
                    CanPlaceOrder = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        #region User Access Providers

        // Manager has permissions for all actions and these in addition
        public static bool CanManageEmployees { get; set; }
        public static bool CanManageCustomers { get; set; }
        public static bool CanManageSuppliers { get; set; }
        public static bool CanManageInventory { get; set; }
        public static bool CanGenerateTransactionReport { get; set; }
        public static bool NotifyDueCreditBills { get; set; }
        public static bool CanViewActivityLog { get; set; }


        //Inventory Manager specific
        public static bool CanPlaceSupplyOrder { get; set; }
        public static bool CanManageReturns { get; set; }
        public static bool NotifyLowStocks { get; set; }

        //Cashier specific
        public static bool CanPlaceOrder { get; set; }
        public static bool CanSettleCreditBillsAndViewDetails { get; set; }

        #endregion
    }
}