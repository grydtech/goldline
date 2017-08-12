using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Data;
using Core.Data.Suppliers;
using Core.Domain.Model.Suppliers;

namespace Core.Domain.Handlers
{
    public class SupplierHandler
    {
        /// <summary>
        ///     Add a new supplier and return if successful
        /// </summary>
        /// <param name="supplier"></param>
        public void AddNewSupplier(Supplier supplier)
        {
            using (var scope = new TransactionScope())
            {
                using (var connection = Connector.GetConnection())
                {
                    var supplierDal = new SupplierDal(connection);
                    supplierDal.Insert(supplier.Name, supplier.Contact);
                    supplier.Id = supplierDal.GetLastInsertId();

                    // Exception handling
                    if (supplier.Id == null)
                        throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id null after insert");
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Search a list of suppliers matching search parameters
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="name"></param>
        public IEnumerable<Supplier> GetSuppliers(uint? supplierId = null, string name = null)
        {
            using (var connection = Connector.GetConnection())
            {
                return new SupplierDal(connection).Search(supplierId, name == null ? null : $"%{name}%").ToList();
            }
        }

        /// <summary>
        ///     Updates supplier name and contact
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="name"></param>
        /// <param name="contact"></param>
        public void UpdateSupplierDetails(Supplier supplier, string name = null, string contact = null)
        {
            // Exception handling
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id is null");

            using (var connection = Connector.GetConnection())
            {
                new SupplierDal(connection).Update(supplier.Id.Value, name, contact);
            }
        }

        /// <summary>
        ///     Try to delete supplier. Return if successful
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public void DeleteSupplier(Supplier supplier)
        {
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier id is null");

            using (var connection = Connector.GetConnection())
            {
                new SupplierDal(connection).Delete(supplier.Id.Value);
            }
        }
    }
}