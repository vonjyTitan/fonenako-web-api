using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace fonenako_service.Controllers
{
    public class JsonQueryBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;
                if (value == null)
                {
                    return Task.CompletedTask;
                }

                var parsed = JsonSerializer.Deserialize(value, bindingContext.ModelType, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                bindingContext.Result = ModelBindingResult.Success(parsed);
            }
            catch (JsonException e)
            {
                bindingContext.ActionContext.ModelState.TryAddModelError(key: e.Path, exception: e,
                    bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}