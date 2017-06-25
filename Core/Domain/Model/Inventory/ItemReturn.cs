﻿using System;
using Core.Domain.Enums;

namespace Core.Domain.Model.Inventory
{
    public class ItemReturn
    {
        public ItemReturn(uint itemId, uint customerId, uint returnQty, bool isHandled, string note)
        {
            ItemId = itemId;
            CustomerId = customerId;
            ReturnQty = returnQty;
            IsHandled = isHandled;
            Note = note;
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
        public bool IsHandled { get; set; }
        public string Note { get; set; }
        public uint UserId { get; set; }
    }
}