using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class ByteTypeEntity : TypeEntity
    {
        public ByteTypeEntity()
        {
            Type = "string";
            Format = "byte";
            SystemType = "System.Byte";
        }
    }
}
