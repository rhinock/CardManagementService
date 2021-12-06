using CMS.Enums;

namespace CMS.ResponseModels
{
    public class ResponseModel
    {
        public BusinessResult Result { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{(int)Result} - {Result}: {Message}";
        }
    }
}
