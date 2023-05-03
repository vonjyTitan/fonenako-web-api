
namespace fonenako_service.Controllers.Validator
{
    public interface IPageableEndPointInputValidator
    {
        bool IsValide(PageableRequestParam pageableRequestParam, out ValidationError firstError);
    }
}
