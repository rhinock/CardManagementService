using System.Collections.Generic;

namespace DataServices.Objects
{
    public class ItemsSearchResult<T>
    {
        public IEnumerable<T> Value { get; set; }
    }
}
