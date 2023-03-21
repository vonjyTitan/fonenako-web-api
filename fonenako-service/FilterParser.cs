using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using fonenako_service.Exceptions;

namespace fonenako_service
{
    public class FilterParser : IFilterParser
    {

        public IDictionary<string, object> ParseFilter(string filterParam, IDictionary<string, Type> filterableFieldMap)
        {
            if(filterableFieldMap == null)
            {
                throw new ArgumentNullException(nameof(filterableFieldMap));
            }
            var keyWithInvalidMappings = filterableFieldMap.Where(map => map.Value == null).Select(keyValue => keyValue.Key).ToArray();
            if(keyWithInvalidMappings.Any())
            {
                throw new ArgumentException($"{filterableFieldMap} contains some mappings with null as target type : {string.Join(", ", keyWithInvalidMappings)}");
            }

            var filterMap = new Dictionary<string, object>();
            foreach (var param in filterParam?.Split("&"))
            {
                var split = param.Split("=");
                var fieldName = split[0];
                if (!filterableFieldMap.TryGetValue(fieldName, out var type))
                {
                    throw new UnknownFilterFieldException(fieldName);
                }

                if (filterMap.TryGetValue(fieldName, out _))
                {
                    throw new DuplicateFilterFieldException(fieldName);
                }

                if (split.Length < 2 || string.IsNullOrWhiteSpace(split[1]))
                {
                    throw new InvalidFilterFieldValueException(fieldName, string.Empty);
                }

                object parsed;
                try
                {
                    parsed = TypeDescriptor.GetConverter(type).ConvertFromString(split[1]);
                }
                catch(ArgumentException)
                {
                    throw new InvalidFilterFieldValueException(fieldName, split[1]);
                }

                filterMap.Add(fieldName, parsed);
            }

            return filterMap;
        }
    }
}
