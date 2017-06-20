using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Core.Data
{
    internal class ProductDetailDal : Dal
    {
        internal ProductDetailDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a new tyre brand or ignores if exists
        /// </summary>
        /// <param name="tyreBrand"></param>
        internal void InsertTyreBrand(string tyreBrand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into tyres_brands values (@value)",
                new {value = tyreBrand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new tyre dimension or ignores if exists
        /// </summary>
        /// <param name="tyreDimension"></param>
        internal void InsertTyreDimension(string tyreDimension)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into tyres_dimensions values (@value)",
                new {value = tyreDimension});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new country or ignores if exists
        /// </summary>
        /// <param name="country"></param>
        internal void InsertCountry(string country)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into countries values (@value)",
                new { value = country });

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new alloywheel brand or ignores if exists
        /// </summary>
        /// <param name="alloywheelBrand"></param>
        internal void InsertAlloywheelBrand(string alloywheelBrand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into alloywheels_brands values (@value)",
                new {value = alloywheelBrand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new alloywheel dimension or ignores if exists
        /// </summary>
        /// <param name="alloywheelDimension"></param>
        internal void InsertAlloywheelDimension(string alloywheelDimension)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into alloywheels_dimensions values (@value)",
                new {value = alloywheelDimension});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new battery brand or ignores if exists
        /// </summary>
        /// <param name="batteryBrand"></param>
        internal void InsertBatteryBrand(string batteryBrand)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into batteries_brands values (@value)",
                new {value = batteryBrand});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of all records in tyres_brands table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllTyreBrands()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from tyres_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in tyres_dimensions table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllTyreDimensions()
        {
            // Define sql command
            var command = new CommandDefinition("select dimension from tyres_dimensions");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in countries table as strings
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllCountries()
        {
            // Define sql command
            var command = new CommandDefinition("select country from countries");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in alloywheels_brands table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllAlloywheelBrands()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from alloywheels_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in alloywheels_dimensions table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllAlloywheelDimensions()
        {
            // Define sql command
            var command = new CommandDefinition("select dimension from alloywheels_dimensions");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in batteries_brands table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllBatteryBrands()
        {
            // Define sql command
            var command = new CommandDefinition("select brand from batteries_brands");

            // Execute sql command
            return Connection.Query<string>(command);
        }
    }
}