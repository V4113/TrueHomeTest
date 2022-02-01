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
    public class ActivityRepository : IActivityRepository
    {

        private PostgreSQLConfiguration _connectionString;
        public ActivityRepository(PostgreSQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> InsertActivity(Activity activity)
        {
            var db = dbConnection();

            
            var sql = @" insert into activity (property_id, schedule, title, created_at, updated_at, status)
                        values (@Property_id, @Schedule, @Title, @Created_at, @Updated_at, @Status);";

            var result = await db.ExecuteAsync(sql, new 
            { 
                Property_id = activity.property_id,
                Schedule = activity.schedule,
                Title = activity.title,
                Created_at = activity.created_at,
                Updated_at = activity.updated_at,
                Status = activity.status
            });

            return result > 0;
            
        }

        public Task<bool> UpdateActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Activity>> GetAllActivityOfProperty(Activity activity)
        {
            var db = dbConnection();
            var sql = @"select * from activity where property_id = @Property_Id";
            return await db.QueryAsync<Activity> (sql, new { Property_Id = activity.property_id });
        }

        public async Task<Activity> GetActivityById(int id)
        {
            var db = dbConnection();
            var sql = @"select * from activity where id = @Id";
            return await db.QueryFirstOrDefaultAsync<Activity>(sql, new { Id = id });
        }

        public async Task<bool> ReagendarActividad(Activity activity)
        {
            var db = dbConnection();

            var sql = @"update activity set schedule = @Schedule where id = @Id";
            var result = await db.ExecuteAsync(sql, new { Schedule = activity.schedule, Id = activity.Id });
            return result > 0;
        }

        public async Task<bool> CancelarActivity(int id)
        {
            var db = dbConnection();

            var sql = @"update activity set status = @Status where id = @Id";
            var result = await db.ExecuteAsync(sql, new { Status = "cancel", Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Activity>> GetListOfActivities(string? d1, string? d2, string? status)
        {
            var db = dbConnection();

            var sql = @"
                select * From activity as a where 
            ";

            string where = ((d1 == null || d2 == null)) ? " ( now() - INTERVAL '3 DAY' <= a.schedule  and a.schedule <= now() + INTERVAL '15 DAY' ) " : " (a.schedule >= '" + d1 + "' and a.schedule < '" + d2 + "') ";

            where += (status == null || status == "") ?  "" : " and ( a.status = '" + status + "' )  " ;
            sql += where;



            return await db.QueryAsync<Activity>(sql, new { });

        }
    }
}
