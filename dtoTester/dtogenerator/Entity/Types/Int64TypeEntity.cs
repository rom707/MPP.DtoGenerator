using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class Int64TypeEntity : TypeEntity
    {
        public Int64TypeEntity()
        {
            Type = "integer";
            Format = "int64";
            SystemType = "System.Int64";
        }
    }
}
