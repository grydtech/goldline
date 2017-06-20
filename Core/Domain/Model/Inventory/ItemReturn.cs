using System;
using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public class ItemReturn
    {
        public ItemReturn(uint itemId, uint customerId, uint returnQty, ReturnCondition condition, string note,
            uint userId)
        {
            ItemId = itemId;
            CustomerId = customerId;
            ReturnQty = returnQty;
            Condition = condition;
            Note = note;
            UserId = userId;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public ItemReturn()
        {
        }

        public uint? Id { get; set; }
        public uint ItemId { get; set; }
        public uint? CustomerId { get; set; }
        public uint ReturnQty { get; set; }
        public DateTime Date { get; set; }
        public ReturnCondition Condition { get; set; }
        public string Note { get; set; }
        public uint UserId { get; set; }
    }
}