using System;
namespace fonenako_service.Exceptions
{
    public class FilterException : Exception
    {
        public string FieldName { get; }

        public FilterException(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
