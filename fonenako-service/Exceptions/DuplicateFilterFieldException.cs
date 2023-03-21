
namespace fonenako_service.Exceptions
{
    public class DuplicateFilterFieldException : FilterException
    {
        public DuplicateFilterFieldException(string fieldName) : base(fieldName)
        {
        }
    }
}
