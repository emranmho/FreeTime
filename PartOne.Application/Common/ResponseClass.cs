using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartOne.Application.Common
{
    public class ResponseClass<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Value { get; set; }
    }
}
