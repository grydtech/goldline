using System;
using System.Collections.Generic;
using Core.Data;
using Core.Model.Enums;
using Core.Security;

namespace Core.Model.Handlers
{
    public class ItemReturnHandler
    {
        /// <summary>
        ///     Adds a new Item return
        /// </summary>
        /// <param name="itemreturn"></param>
        public void AddNewItemReturn(ItemReturn itemreturn)
        {
            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).InsertItemReturn(itemreturn, User.CurrentUser.EmployeeId);
            }
        }

        /// <summary>
        ///     Updates an item return record with new attributes
        /// </summary>
        /// <param name="itemreturn"></param>
        public void UpdateItemReturn(ItemReturn itemreturn)
        {
            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).UpdateItemReturnDetails(itemreturn);
            }
        }

        public void UpdateItemReturnStatus(ItemReturn itemReturn, ReturnCondition returnCondition)
        {
            // Exception handling
            if (itemReturn.Id == null) throw new ArgumentNullException(nameof(itemReturn.Id), "Item return Id is null");

            using (var connection = Connector.GetConnection())
            {
                new ItemReturnDal(connection).UpdateItemReturnCondition((uint) itemReturn.Id, returnCondition);
            }
        }

        /// <summary>
        ///     Returns a list of all item returns
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemReturn> GetAllItemReturns(DateTime startDate, DateTime endDate)
        {
            using (var connection = Connector.GetConnection())
            {
                return new ItemReturnDal(connection).GetAllItemReturns(startDate, endDate);
            }
        }

        /// <summary>
        ///     Returns list of ItemReturns based on given search parameters. If condition not specified, returns all
        /// </summary>
        /// <param name="note"></param>
        /// <param name="condition"></param>
        /// <param name="isLimited">By default the limit is 20 records. Mark this as false to get all records</param>
        /// <returns></returns>
        public IEnumerable<ItemReturn> SearchItemReturns(string note, ReturnCondition? condition = null,
            bool isLimited = true)
        {
            using (var connection = Connector.GetConnection())
            {
                return new ItemReturnDal(connection).GetItemReturns(note, condition,
                    isLimited ? Constraints.DefaultLimit : Constraints.ExtendedLimit);
            }
        }
    }
}