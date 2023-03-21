
namespace fonenako_service.Exceptions
{
    public class InvalidFilterFieldValueException : FilterException
    {
        public object InvalidValue { get; }

        public InvalidFilterFieldValueException(string fieldName, object invalidValue) : base(fieldName)
        {
            InvalidValue = invalidValue;
        }
    }
}
