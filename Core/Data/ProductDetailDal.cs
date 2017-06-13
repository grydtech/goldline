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
        ///     Inserts a new battery capacity or ignores if exists
        /// </summary>
        /// <param name="batteryCapacity"></param>
        internal void InsertBatteryCapacity(uint batteryCapacity)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into batteries_capacities values (@value)",
                new {value = batteryCapacity});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Inserts a new battery voltage or ignores if exists
        /// </summary>
        /// <param name="batteryVoltage"></param>
        internal void InsertBatteryVoltage(uint batteryVoltage)
        {
            // Define sql command
            var command = new CommandDefinition("insert ignore into batteries_voltages values (@value)",
                new {value = batteryVoltage});

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

        /// <summary>
        ///     Returns a list of all records in batteries_capacities table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllBatteryCapacities()
        {
            // Define sql command
            var command = new CommandDefinition("select capacity from batteries_capacities");

            // Execute sql command
            return Connection.Query<string>(command);
        }

        /// <summary>
        ///     Returns a list of all records in batteries_voltages table as string
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAllBatteryVoltages()
        {
            // Define sql command
            var command = new CommandDefinition("select voltage from batteries_voltages");

            // Execute sql command
            return Connection.Query<string>(command);
        }
    }
}