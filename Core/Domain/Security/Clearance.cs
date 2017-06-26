using System;
using Core.Domain.Enums;

namespace Core.Domain.Security
{
    public class Clearance
    {
        public Clearance(AccessMode userType)
        {
            CanManageCustomers = false;
            CanManageProducts = false;
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
                case AccessMode.Manager:
                    CanManageCustomers = true;
                    CanManageProducts = true;
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
                case AccessMode.InventoryManager:
                    CanManageReturns = true;
                    CanPlaceSupplyOrder = true;
                    NotifyLowStocks = true;
                    break;
                case AccessMode.Cashier:
                    CanSettleCreditBillsAndViewDetails = true;
                    CanPlaceOrder = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        #region User AccessMode Providers

        // Manager has permissions for all actions and these in addition
        public bool CanManageEmployees { get; set; }

        public bool CanManageCustomers { get; set; }
        public bool CanManageSuppliers { get; set; }
        public bool CanManageProducts { get; set; }
        public bool CanGenerateTransactionReport { get; set; }
        public bool NotifyDueCreditBills { get; set; }
        public bool CanViewActivityLog { get; set; }


        //Inventory Manager specific
        public bool CanPlaceSupplyOrder { get; set; }

        public bool CanManageReturns { get; set; }
        public bool NotifyLowStocks { get; set; }

        //Cashier specific
        public bool CanPlaceOrder { get; set; }

        public bool CanSettleCreditBillsAndViewDetails { get; set; }

        #endregion
    }
}