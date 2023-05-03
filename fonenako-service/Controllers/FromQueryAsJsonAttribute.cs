
using Microsoft.AspNetCore.Mvc;

namespace fonenako_service.Controllers
{
    public class FromQueryAsJsonAttribute : ModelBinderAttribute
    {
        public FromQueryAsJsonAttribute()
        {
            BinderType = typeof(JsonQueryBinder);
        }
    }
}
