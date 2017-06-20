using System.Data;

namespace Core.Data.Suppliers
{
    internal class PurchasePaymentDal : Dal
    {
        internal PurchasePaymentDal(IDbConnection connection) : base(connection)
        {
        }
    }
}