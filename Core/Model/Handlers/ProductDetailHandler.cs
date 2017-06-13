using System.Collections.Generic;
using Core.Data;

namespace Core.Model.Handlers
{
    public class ProductDetailHandler
    {
        /// <summary>
        ///     Adds a new alloywheel brand or ignore if already exists
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        public void AddNewAlloywheelBrand(string brandName)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertAlloywheelBrand(brandName);
            }
        }

        /// <summary>
        ///     Adds a new alloywheel dimension or ignore if already exists
        /// </summary>
        /// <param name="alloywheelDimension"></param>
        /// <returns></returns>
        public void AddNewAlloywheelDimension(string alloywheelDimension)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertAlloywheelDimension(alloywheelDimension);
            }
        }

        /// <summary>
        ///     Adds a new tyre brand or ignore if already exists
        /// </summary>
        /// <param name="tyreBrand"></param>
        /// <returns></returns>
        public void AddNewTyreBrand(string tyreBrand)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertTyreBrand(tyreBrand);
            }
        }

        /// <summary>
        ///     Adds a new tyre dimension or ignore if already exists
        /// </summary>
        /// <param name="tyreDimension"></param>
        /// <returns></returns>
        public void AddNewTyreDimension(string tyreDimension)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertTyreDimension(tyreDimension);
            }
        }

        /// <summary>
        ///     Adds a new battery brand or ignore if already exists
        /// </summary>
        /// <param name="batteryBrand"></param>
        /// <returns></returns>
        public void AddNewBatteryBrand(string batteryBrand)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertBatteryBrand(batteryBrand);
            }
        }

        /// <summary>
        ///     Adds a new battery capacity or ignore if already exists
        /// </summary>
        /// <param name="batteryCapacity"></param>
        /// <returns></returns>
        public void AddNewBatteryCapacity(uint batteryCapacity)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertBatteryCapacity(batteryCapacity);
            }
        }

        /// <summary>
        ///     Adds a new battery voltage or ignore if already exists
        /// </summary>
        /// <param name="batteryVoltage"></param>
        /// <returns></returns>
        public void AddNewBatteryVoltage(uint batteryVoltage)
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                new ProductDetailDal(connection).InsertBatteryVoltage(batteryVoltage);
            }
        }

        /// <summary>
        ///     Returns a list of all stored tyre brands
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllTyreBrands()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllTyreBrands();
            }
        }

        /// <summary>
        ///     Returns a list of all stored tyre dimensions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllTyreDimensions()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllTyreDimensions();
            }
        }

        /// <summary>
        ///     Returns a list of all stored alloywheel brands
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllAlloywheelBrands()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllAlloywheelBrands();
            }
        }

        /// <summary>
        ///     Returns a list of all stored alloywheel dimensions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllAlloywheelDimensions()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllAlloywheelDimensions();
            }
        }

        /// <summary>
        ///     Returns a list of all stored battery brands
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllBatteryBrands()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllBatteryBrands();
            }
        }

        /// <summary>
        ///     Returns a list of all stored battery capacities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllBatteryCapacities()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllBatteryCapacities();
            }
        }

        /// <summary>
        ///     Returns a list of all stored battery voltages
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllBatteryVoltages()
        {
            using (var connection = ConnectionManager.GetConnection())
            {
                return new ProductDetailDal(connection).GetAllBatteryVoltages();
            }
        }
    }
}