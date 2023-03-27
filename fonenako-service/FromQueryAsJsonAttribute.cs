
using Microsoft.AspNetCore.Mvc;

namespace fonenako_service
{
    public class FromQueryAsJsonAttribute : ModelBinderAttribute
    {
        public FromQueryAsJsonAttribute()
        {
            BinderType = typeof(JsonQueryBinder);
        }
    }
}
