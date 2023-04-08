using System.Collections.Generic;

namespace Project.Application
{
    public class BaseCollectionResult<T>
    {
        public IEnumerable<T> Result { get; set; }

        public long TotalCount { get; set; }
    }
}