using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTrueHome.Model;

namespace TestTrueHome.Data.Repositories
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllProperties();
        Task<Property> GetProperty(int id);
        Task<bool> InsertProperty(Property property);
        Task<bool> UpdateProperty(Property property);
        Task<bool> DeleteProperty(Property property);


    }
}
