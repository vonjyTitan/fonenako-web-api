
using System.Collections.Generic;
using System.Threading.Tasks;
using fonenako_service.Models;

namespace fonenako_service.Daos
{
    public interface ILocalisationDao
    {
        Task<IEnumerable<Localisation>> SearchLocalisationsAsync(int pageSize, int pageIndex, string nameFilter, string orderBy, Order order);

        Task<int> CountLocalisationsFoundAsync(string nameFilter);
    }
}
