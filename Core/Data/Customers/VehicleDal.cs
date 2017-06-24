using System;
using System.Collections.Generic;
using System.Data;
using Core.Domain.Model.Customers;
using Dapper;

namespace Core.Data.Customers
{
    internal class VehicleDal : Dal
    {
        internal VehicleDal(IDbConnection connection) : base(connection)
        {
        }

        /// <summary>
        ///     Inserts a record into [vehicles] table
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <param name="customerId"></param>
        internal void Insert(string vehicleNo, uint customerId)
        {
            // Define sql command
            var command = new CommandDefinition(
                "insert into customers_vehicles (vehicle_no, id_customer) values (@vehicleNo, @customerId)",
                new {vehicleNo, customerId});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Searches for records in [vehicles] table
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal IEnumerable<Vehicle> Search(uint? customerId = null, int offset = 0, int limit = int.MaxValue)
        {
            // Define sql command
            var command = new CommandDefinition(
                "select vehicle_no 'Number', id_customer 'CustomerId', date_last_seen 'LastSeenDate' from customers_vehicles " +
                (customerId == null ? "" : "where id_customer = @customerId ") +
                "order by vehicle_no limit @offset, @limit",
                new {customerId, offset, limit});

            // Execute sql command
            return Connection.Query<Vehicle>(command);
        }

        /// <summary>
        ///     Updates a record in [vehicles] table
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <param name="lastSeenDate"></param>
        internal void Update(string vehicleNo, DateTime lastSeenDate)
        {
            // Define sql command
            var command = new CommandDefinition(
                "update customers_vehicles set date_last_seen = @lastSeenDate where vehicle_no = @vehicleNo",
                new {vehicleNo, lastSeenDate});

            // Execute sql command
            Connection.Execute(command);
        }

        /// <summary>
        ///     Deletes a record from [vehicles] table
        /// </summary>
        /// <param name="vehicleNo"></param>
        internal void Delete(string vehicleNo)
        {
            // Define sql command
            var command = new CommandDefinition(
                "delete from customers_vehicles where vehicle_no = @vehicleNo",
                new {vehicleNo});

            // Execute sql command
            Connection.Execute(command);
        }
    }
}