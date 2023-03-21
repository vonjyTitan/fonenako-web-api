
using System;
using System.Collections.Generic;

namespace fonenako_service
{
    public interface IFilterParser
    {
        IDictionary<string, object> ParseFilter(string filterParam, IDictionary<string, Type> filterableFieldMap);
    }
}
