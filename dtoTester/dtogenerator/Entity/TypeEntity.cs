using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity
{
    public abstract class TypeEntity
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string SystemType { get; set; }
    }
}
