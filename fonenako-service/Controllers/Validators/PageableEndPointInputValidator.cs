using System;
using System.Collections.Generic;
using fonenako_service.Controllers.Validators;
using fonenako_service.Properties;

namespace fonenako_service.Controllers.Validator
{
    public class PageableEndPointInputValidator : IPageableEndPointInputValidator
    {
        private readonly ISet<string> SortableFields;

        public PageableEndPointInputValidator(ISet<string> sortableFields)
        {
            SortableFields = sortableFields ?? throw new ArgumentNullException(nameof(sortableFields));
        }

        public bool IsValide(PageableRequestParam pageableRequestParam, out ValidationError firstError)
        {
            firstError = null;
            if (pageableRequestParam.Order.HasValue && string.IsNullOrWhiteSpace(pageableRequestParam.OrderBy))
            {
                firstError = new ValidationError
                {
                    Message = string.Format(Resources.order_whithout_orderby, pageableRequestParam.Order, pageableRequestParam.OrderBy),
                    ErrorCode = ErrorCode.OrderWihoutDirection
                };
                return false;
            }

            if (!string.IsNullOrWhiteSpace(pageableRequestParam.OrderBy) && !SortableFields.Contains(pageableRequestParam.OrderBy))
            {
                firstError = new ValidationError
                {
                    Message = string.Format(Resources.unknown_order_field_name, pageableRequestParam.OrderBy),
                    ErrorCode = ErrorCode.UnknownSortField
                };
                return false;
            }

            if (pageableRequestParam.Page.HasValue && pageableRequestParam.Page < 1)
            {
                firstError = new ValidationError
                {
                    Message = string.Format(Resources.requested_page_index_not_valid, pageableRequestParam.Page),
                    ErrorCode = ErrorCode.InvalidPageIndex
                };
                return false;
            }

            if (pageableRequestParam.PageSize.HasValue && pageableRequestParam.PageSize < 1)
            {
                firstError = new ValidationError
                {
                    Message = string.Format(Resources.requested_page_size_not_valid, pageableRequestParam.PageSize),
                    ErrorCode = ErrorCode.InvalidPageSize
                };
                return false;
            }

            return true;
        }
    }
}
