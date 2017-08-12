using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Data.Inventory;
using Core.Domain.Model.Inventory;

namespace Core.Domain.Handlers
{
    public class ItemReturnHandler
    {
        /// <summary>
        ///     Adds a new Item return
        /// </summary>
        /// <param name="itemreturn"></param>
        public void AddItemReturn(ItemReturn itemreturn)
        {
            if (string.IsNullOrEmpty(itemreturn.ContactInfo))
                throw new ArgumentNullException(nameof(itemreturn.CustomerId), "ItemReturn Contact Information is null");
            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).Insert(itemreturn.ItemId, itemreturn.CustomerId, itemreturn.ContactInfo,
                    itemreturn.ReturnQty, itemreturn.IsHandled, itemreturn.Note);
            }
        }

        /// <summary>
        ///     Updates an item return record with new attributes
        /// </summary>
        /// <param name="itemreturn"></param>
        /// <param name="customerId"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="isHandled"></param>
        /// <param name="note"></param>
        public void UpdateItemReturn(ItemReturn itemreturn, uint? customerId = null, uint? itemId = null,
            uint? qty = null, bool? isHandled = null,
            string note = null)
        {
            if (itemreturn.Id == null)
                throw new ArgumentNullException(nameof(itemreturn.CustomerId), "ItemReturn Id is null");

            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).Update(itemreturn.Id.Value, customerId, itemId, qty, isHandled, note);
                // Update object if successful
                itemreturn.CustomerId = customerId ?? itemreturn.CustomerId;
                itemreturn.ItemId = itemId ?? itemreturn.ItemId;
                itemreturn.ReturnQty = qty ?? itemreturn.ReturnQty;
                itemreturn.IsHandled = isHandled ?? itemreturn.IsHandled;
            }
        }

        /// <summary>
        ///     Returns a list of all item returns
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemReturn> GetItemReturns(string note = null, bool? isHandled = null,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var connection = Connector.GetConnection())
            {
                var itemReturns =  new ItemReturnDal(connection).Search($"%{note}%", isHandled, startDate, endDate).ToList();
                var productHandler = new ProductHandler();

                foreach (var itemReturn in itemReturns)
                {
                    itemReturn.ItemName = productHandler.GetItems(itemReturn.ItemId).SingleOrDefault()?.Name;
                }
                return itemReturns;
            }
        }
    }
}