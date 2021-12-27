using GatewayService.Enums;

namespace GatewayService.ResponseModels
{
    public class ResponseModel
    {
        public BusinessResult Result { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{{ Code: {Result}, Message: {Message} }}";
        }
    }
}
