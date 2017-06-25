using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Data;
using Core.Data.Suppliers;
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
            if (purchase.SupplierId == null)
                throw new ArgumentNullException(nameof(purchase.SupplierId), "Purchase SupplierId is null");
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var purchaseDal = new PurchaseDal(connection);
                    purchaseDal.Insert(purchase.SupplierId.Value, purchase.Amount, purchase.Note, purchase.IsSettled);
                    purchase.Id = purchaseDal.GetLastInsertId();

                    if (purchase.Id == null)
                        throw new ArgumentNullException(nameof(purchase.Id),
                            "Purchase Id null after insert");

                    var purchaseItemDal = new PurchaseItemDal(connection);
                    purchaseItemDal.InsertMultiple(purchase.Id.Value, purchase.PurchaseItems);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Returns list of purchases matching the given parameters
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="isSettled"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public IEnumerable<Purchase> GetPurchases(uint? supplierId = null, bool? isSettled = null, string note = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new PurchaseDal(connection).Search(supplierId, isSettled, note);
            }
        }

        /// <summary>
        ///     Updates a purchase
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="supplierId"></param>
        /// <param name="amount"></param>
        /// <param name="isSettled"></param>
        /// <param name="note"></param>
        /// <param name="purchaseItems"></param>
        public void UpdatePurchase(Purchase purchase, uint? supplierId = null, decimal? amount = null,
            bool? isSettled = null, string note = null, IEnumerable<PurchaseItem> purchaseItems = null)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    if (purchase.Id == null)
                        throw new ArgumentNullException(nameof(purchase.Id), "Purchase Id is null");

                    var purchaseDal = new PurchaseDal(connection);
                    purchaseDal.Update(purchase.Id.Value, supplierId, amount, isSettled, note);

                    var purchaseItemsList = purchaseItems?.ToList();
                    if (purchaseItemsList?.Any() == true)
                    {
                        var purchaseItemDal = new PurchaseItemDal(connection);
                        purchaseItemDal.Delete(purchase.Id.Value);
                        purchaseItemDal.InsertMultiple(purchase.Id.Value, purchaseItemsList);
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Update a set of purchases
        /// </summary>
        /// <param name="purchases"></param>
        /// <param name="isSettled"></param>
        public void UpdatePurchaseMultiple(IEnumerable<Purchase> purchases, bool isSettled)
        {
            var purchasesList = purchases.ToList();
            if (purchasesList.Any(p => p.Id == null))
                throw new ArgumentNullException(nameof(purchasesList), "Some Purchases have null Id");

            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var purchaseDal = new PurchaseDal(connection);
                    foreach (var purchase in purchasesList)
                        purchaseDal.Update(purchase.Id.GetValueOrDefault(), isSettled: isSettled);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Deletes a purchase
        /// </summary>
        /// <param name="purchaseId"></param>
        public void DeletePurchase(uint purchaseId)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var purchaseDal = new PurchaseDal(connection);
                    purchaseDal.Delete(purchaseId);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Load purchaseItems of a purchase
        /// </summary>
        /// <param name="purchase"></param>
        public void LoadPurchaseItems(Purchase purchase)
        {
            if (purchase.Id == null)
                throw new ArgumentNullException(nameof(purchase.Id), "Purchase Id is null");

            using (var connection = Connector.GetConnection())
            {
                var purchaseItems = new PurchaseItemDal(connection).Search(purchase.Id.Value);
                purchase.PurchaseItems = purchaseItems?.ToList();
            }
        }
    }
}