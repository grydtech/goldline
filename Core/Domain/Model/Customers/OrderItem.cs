namespace Core.Domain.Model.Customers
{
    public class OrderItem
    {
        public OrderItem(uint productId, string productName, uint qty, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Qty = qty;
        }

        /// <summary>
        ///     For database initialization
        /// </summary>
        public OrderItem()
        {
        }

        public uint ProductId { get; }
        public string ProductName { get; }
        public uint Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetPrice => UnitPrice * Qty;
    }
}