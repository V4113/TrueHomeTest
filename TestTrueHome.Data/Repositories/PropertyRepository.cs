using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTrueHome.Model;

namespace TestTrueHome.Data.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private PostgreSQLConfiguration _connectionString;
        public PropertyRepository(PostgreSQLConfiguration connectionString)
        { 
            _connectionString = connectionString;
        }
        protected NpgsqlConnection dbConnection() 
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public Task<bool> DeleteProperty(Property property)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Property>> GetAllProperties()
        {
            var db = dbConnection();
            var sql = @"select * from property";
            return await db.QueryAsync<Property>(sql, new { });
        }

        public async Task<Property> GetProperty(int id)
        {
            var db = dbConnection();
            var sql = @"select * from property where id = @Id; ";

            return await db.QueryFirstOrDefaultAsync<Property>(sql, new { Id = id });
        }

        public async Task<bool> InsertProperty(Property property)
        {
            var db = dbConnection();
            var sql = @" insert into  property (title, address, description, created_at, updated_at, disabled_at, status)
                        values ('" + property.title + "', '" + property.address + "','" + property.description + "', now() - INTERVAL '4 DAY', now(), null, 'active');";
            var result = await db.ExecuteAsync(sql, new { });
            return result > 0;

        }

        public Task<bool> UpdateProperty(Property property)
        {
            throw new NotImplementedException();
        }
    }
}
