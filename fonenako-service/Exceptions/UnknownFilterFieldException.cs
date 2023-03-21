
namespace fonenako_service.Exceptions
{
    public class UnknownFilterFieldException : FilterException
    {
        public UnknownFilterFieldException(string fieldName) : base(fieldName)
        {
        }
    }
}
