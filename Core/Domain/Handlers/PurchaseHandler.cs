using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Data;
using Core.Data.Suppliers;
using Core.Domain.Model.Employees;
using Core.Domain.Model.Suppliers;

namespace Core.Domain.Handlers
{
    public class PurchaseHandler
    {
        /// <summary>
        ///     Adds a new purchase
        /// </summary>
        /// <param name="purchase"></param>
        public void AddPurchase(Purchase purchase)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var supplyOrderDal = new PurchaseDal(connection);
                    supplyOrderDal.Insert(purchase, User.CurrentUser.EmployeeId);

                    if (purchase.Id == null)
                        throw new ArgumentNullException(nameof(purchase.Id),
                            "Supply order Id has not been assigned after insert");

                    supplyOrderDal.InsertPurchasedItems((uint)purchase.Id, purchase.OrderEntries);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Returns list of supplyorders matching the given note text
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public IEnumerable<Purchase> GetPurchases(uint? supplierId = null, string note = null, bool? isSettled = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search(note);
            }
        }

        /// <summary>
        ///     Updates a supply order
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="isEntriesUpdated">If order entries need to be updated, set this true</param>
        public void UpdatePurchase(Purchase purchase, decimal? amount = null, bool? isSettled = null, string note = null)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    if (purchase.Id == null)
                        throw new ArgumentNullException(nameof(purchase.Id), "Supply Order Id is null");

                    var supplyOrderDal = new PurchaseDal(connection);
                    supplyOrderDal.Update(purchase);

                    if (!isEntriesUpdated) return;

                    if (purchase.OrderEntries == null)
                        throw new ArgumentNullException(
                            nameof(purchase.OrderEntries),
                            "Attempt to update order entries while OrderItems = null");

                    supplyOrderDal.RemovePurchasedItems((uint)purchase.Id);
                    supplyOrderDal.InsertPurchasedItems((uint)purchase.Id, purchase.OrderEntries);
                }
                scope.Complete();
            }
        }
        
        /// <summary>
        ///     Update a set of supply orders as paid off
        /// </summary>
        /// <param name="purchases"></param>
        public void UpdatePurchaseMultiple(IEnumerable<Purchase> purchases, bool isSettled)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var supplyOrderDal = new PurchaseDal(connection);
                    foreach (var supplyOrder in purchases)
                    {
                        supplyOrder.Status = SupplyOrderStatus.Paid;
                        supplyOrderDal.Update(supplyOrder, SupplyOrderStatus.Paid);
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Load Order Entries of a supply order
        /// </summary>
        /// <param name="purchase"></param>
        public void LoadPurchaseItems(Purchase purchase)
        {
            using (var connection = Connector.GetConnection())
            {
                new PurchaseDal(connection).LoadPurchasedItems(purchase);
            }
        }
    }
}
