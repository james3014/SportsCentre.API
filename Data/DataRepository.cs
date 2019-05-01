using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class DataRepository : IDataRepository
    {
        public Task<Class> GetCurrentClasses()
        {
            throw new System.NotImplementedException();
        }
    }
}