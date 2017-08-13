using System;

namespace Core.Domain.Model.Inventory
{
    public class ItemReturn
    {
        public ItemReturn(uint itemId, uint? customerId, string contactInfo, uint returnQty, bool isHandled,
            string note)
        {
            ItemId = itemId;
            CustomerId = customerId;
            ContactInfo = contactInfo;
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
        public string ItemName { get; set; }
        public uint? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactInfo { get; set; }
        public uint ReturnQty { get; set; }
        public DateTime Date { get; set; }
        public bool IsHandled { get; set; }
        public string Note { get; set; }
    }
}