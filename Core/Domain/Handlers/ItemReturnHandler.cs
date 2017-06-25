﻿using System;
using System.Collections.Generic;
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
            if(itemreturn.CustomerId == null)
                throw new ArgumentNullException(nameof(itemreturn.CustomerId), "ItemReturn CustomerId is null");
            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).Insert(itemreturn.ItemId, itemreturn.CustomerId.Value, itemreturn.ReturnQty, itemreturn.IsHandled, itemreturn.Note);
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
        public void UpdateItemReturn(ItemReturn itemreturn, uint? customerId = null, uint? itemId = null, uint? qty = null, bool? isHandled = null,
            string note = null)
        {
            if (itemreturn.Id == null)
                throw new ArgumentNullException(nameof(itemreturn.CustomerId), "ItemReturn Id is null");

            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).Update(itemreturn.Id.Value, customerId, itemId, qty, isHandled, note);
            }
        }

        /// <summary>
        ///     Returns a list of all item returns
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemReturn> GetItemReturns(string note = null, bool? isHandled = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new ItemReturnDal(connection).Search(note, isHandled, startDate, endDate);
            }
        }
    }
}