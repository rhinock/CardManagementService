using CMS.Enums;

namespace CMS.Models
{
    public class ResponseModel
    {
        public BusinessResult Result { get; set; }
        public string Message { get; set; }

        public ResponseModel()
        {
            Result = BusinessResult.Success;
            Message = "Success";
        }

        public ResponseModel(BusinessResult result, string message)
        {
            Result = result;
            Message = message;
        }
    }
}
