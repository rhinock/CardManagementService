namespace CMS.Models
{
    public class ApiResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public ApiResponseModel()
        {
            Data = default(T);
        }

        public ApiResponseModel(T data)
        {
            Data = data;
        }
    }
}
