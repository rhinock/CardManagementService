namespace CMS.Models
{
    public class ApiModel<T>
    {
        public bool IsOk { get; set; }
        public T Data { get; set; }
    }
}
