
using System.Collections.Generic;

namespace fonenako_service.Controllers.Validator
{
    public class EndPointInputValidatorFactory : IEndPointInputValidatorFactory
    {
        public IPageableEndPointInputValidator CreatePageableEndPointInputValidator(ISet<string> sortableFieldes)
        {
            return new PageableEndPointInputValidator(sortableFieldes);
        }
    }
}
