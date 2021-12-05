using CMS.Enums;

namespace CMS.Models
{
    public class ApiResult
    {
        public ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Result { get; set; }
    }
}
