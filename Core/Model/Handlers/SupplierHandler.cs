using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Data;
using Core.Model.Persons;

namespace Core.Model.Handlers
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
                using (var connection = ConnectionManager.GetConnection())
                {
                    var supplierDal = new SupplierDal(connection);
                    supplierDal.InsertSupplier(supplier);

                    // Exception handling
                    if (supplier.Id == null)
                        throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id null after insert");
                    if (supplier.SuppliedItems.Any(suppliedItem => suppliedItem.Id == null))
                        throw new ArgumentNullException(nameof(supplier.SuppliedItems),
                            "Some items in suppliedItems do not have Id");

                    supplierDal.InsertSuppliedItems((uint) supplier.Id, supplier.SuppliedItems);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Get a list of suppliers matching search parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isSuppliedItemsLoaded"></param>
        public IEnumerable<Supplier> GetSuppliers(string name, bool isSuppliedItemsLoaded = true)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                var suppliers = new SupplierDal(connection).GetSuppliers(name).ToList();

                if (!isSuppliedItemsLoaded)
                    return suppliers;

                foreach (var supplier in suppliers)
                {
                    LoadSuppliedItems(supplier);
                    return suppliers;
                }
            }
            return null;
        }

        /// <summary>
        ///     Updates supplier name and contact
        /// </summary>
        /// <param name="supplier"></param>
        public void UpdateSupplierDetails(Supplier supplier)
        {
            // Exception handling
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id is null");
            if (supplier.Name == null) throw new ArgumentNullException(nameof(supplier.Name), "Supplier Name is null");
            if (supplier.Contact == null)
                throw new ArgumentNullException(nameof(supplier.Contact), "Supplier Contact is null");

            using (var connection = ConnectionManager.GetConnection())
            {
                new SupplierDal(connection).UpdateSupplierDetails(supplier);
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

            using (var connection = ConnectionManager.GetConnection())
            {
                new SupplierDal(connection).RemoveSupplier((uint) supplier.Id);
            }
        }

        /// <summary>
        ///     Updates the supplied items of a supplier
        /// </summary>
        /// <param name="supplier"></param>
        public void UpdateSuppliedItems(Supplier supplier)
        {
            // Exception handling
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id is null");
            if (supplier.SuppliedItems == null)
                throw new ArgumentNullException(nameof(supplier.SuppliedItems), "Supplied items is null");
            if (supplier.SuppliedItems.Any(suppliedItem => suppliedItem.Id == null))
                throw new ArgumentNullException(nameof(supplier.SuppliedItems),
                    "Some items in suppliedItems do not have Id hence could not complete the action");

            using (var scope = new TransactionScope())
            {
                using (var connection = ConnectionManager.GetConnection())
                {
                    var supplierDal = new SupplierDal(connection);
                    supplierDal.ClearSuppliedItems((uint) supplier.Id);
                    supplierDal.InsertSuppliedItems((uint) supplier.Id, supplier.SuppliedItems);
                }
                scope.Complete();
            }
        }

        /// <summary>
        ///     Get a list of supplied items which a supplier provides
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public void LoadSuppliedItems(Supplier supplier)
        {
            if (supplier.Id == null) throw new ArgumentNullException(nameof(supplier.Id), "Supplier Id is null");
            using (var connection = ConnectionManager.GetConnection())
            {
                supplier.SuppliedItems = new SupplierDal(connection).GetSuppliedItems((uint) supplier.Id).ToList();
            }
        }

        /// <summary>
        ///     Returns a list of all suppliers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Supplier> GetAllSuppliers()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new SupplierDal(connection).GetAllSuppliers();
            }
        }
    }
}