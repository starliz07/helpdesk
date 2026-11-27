using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HelpDesk.Data.Infrastructure
{
    public class PostgresConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
