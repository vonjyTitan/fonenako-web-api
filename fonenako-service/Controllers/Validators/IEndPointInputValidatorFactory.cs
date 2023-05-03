
using System.Collections.Generic;

namespace fonenako_service.Controllers.Validator
{
    public interface IEndPointInputValidatorFactory
    {
        IPageableEndPointInputValidator CreatePageableEndPointInputValidator(ISet<string> sortableFieldesMap);
    }
}
