using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTrueHome.Model;

namespace TestTrueHome.Data.Repositories
{
    public interface IActivityRepository
    {

        Task<bool> InsertActivity(Activity activity);

        Task<bool> UpdateActivity(Activity activity);

        Task<IEnumerable<Activity>> GetAllActivityOfProperty(Activity activity);

        Task<Activity> GetActivityById(int id);

        Task<bool> ReagendarActividad(Activity activity);

        Task<bool> CancelarActivity(int id);

        Task<IEnumerable<Activity>> GetListOfActivities(string? d1, string? d2, string? status);

        
    }
}
