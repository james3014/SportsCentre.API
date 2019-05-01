using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IDataRepository
    {
         Task<Class> GetCurrentClasses();
    }
}