using System;
using Core.Domain.Enums;

namespace Core.Domain.Security
{
    public class Clearance
    {
        public Clearance(AccessMode accessMode)
        {
            CanManageCustomers = false;
            CanManageProducts = false;
            CanManageSuppliers = false;
            CanManageEmployees = false;
            CanHandleOrders = false;
            CanHandlePurchases = false;
            CanHandleItemReturns = false;
            NotifyDueCreditBills = false;
            CanHandleOrderPayments = false;
            NotifyLowStocks = false;
            CanGenerateReports = false;
            CanViewActivityLog = false;

            switch (accessMode)
            {
                case AccessMode.Manager:
                    CanManageCustomers = true;
                    CanManageProducts = true;
                    CanManageSuppliers = true;
                    CanHandleOrderPayments = true;
                    CanManageEmployees = true;
                    CanHandleOrders = true;
                    CanHandleItemReturns = true;
                    CanHandlePurchases = true;
                    NotifyDueCreditBills = true;
                    NotifyLowStocks = true;
                    CanGenerateReports = true;
                    CanViewActivityLog = true;
                    break;
                case AccessMode.InventoryManager:
                    CanHandleItemReturns = true;
                    CanHandlePurchases = true;
                    NotifyLowStocks = true;
                    break;
                case AccessMode.Cashier:
                    CanHandleOrderPayments = true;
                    CanHandleOrders = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessMode), accessMode, null);
            }
        }

        #region User AccessMode Providers

        // Manager has permissions for all actions and these in addition
        public bool CanManageEmployees { get; set; }

        public bool CanManageCustomers { get; set; }
        public bool CanManageSuppliers { get; set; }
        public bool CanManageProducts { get; set; }
        public bool CanGenerateReports { get; set; }
        public bool NotifyDueCreditBills { get; set; }
        public bool CanViewActivityLog { get; set; }


        //Inventory Manager specific
        public bool CanHandlePurchases { get; set; }

        public bool CanHandleItemReturns { get; set; }
        public bool NotifyLowStocks { get; set; }

        //Cashier specific
        public bool CanHandleOrders { get; set; }

        public bool CanHandleOrderPayments { get; set; }

        #endregion
    }
}