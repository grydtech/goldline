﻿using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class VehicleDal : Dal
    {
        internal VehicleDal(IDbConnection connection, IDbTransaction transaction = null) : base(connection, transaction)
        {
        }

        /// <summary>
        ///     Inserts a new customer vehicle into database or update last visited date if record exists
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="vehicleNo"></param>
        internal void InsertVehicle(uint customerId, string vehicleNo)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers_vehicles (vehicle_no, id_customer) values (@vehicleNo, @customerId)",
                new {vehicleNo, customerId},
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Updates last visited date of existing customer vehicle in database
        /// </summary>
        /// <param name="vehicleNo"></param>
        internal void UpdateVehicleVisited(string vehicleNo)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update customers_vehicles set date_visited = CURRENT_TIMESTAMP where vehicle_no = @vehicleNo",
                new {vehicleNo},
                Transaction);

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Returns a list of customer vehicles matching the customer's Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        internal IEnumerable<Vehicle> GetVehicles(uint customerId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select vehicle_no 'Number', id_customer 'CustomerId', date_visited 'LastVisitDate' from customers_vehicles " +
                "where id_customer = @customerId " +
                "order by vehicle_no",
                new {customerId},
                Transaction);

            // Execute sql command
            return Connection.Query<Vehicle>(command);
        }
    }
}