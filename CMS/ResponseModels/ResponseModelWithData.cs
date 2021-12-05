namespace CMS.ResponseModels
{
    public class ResponseModelWithData<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
