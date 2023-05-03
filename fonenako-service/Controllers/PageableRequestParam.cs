
using fonenako_service.Daos;

namespace fonenako_service.Controllers
{
    public class PageableRequestParam
    {
        public int? PageSize { get; set; }

        public int? Page { get; set; }

        public string OrderBy { get; set; }

        public Order? Order { get; set; }
    }
}
