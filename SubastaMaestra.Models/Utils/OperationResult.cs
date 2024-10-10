using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.Utils
{
    public class OperationResult <T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Value { get; set; }
        
    }

    
}
