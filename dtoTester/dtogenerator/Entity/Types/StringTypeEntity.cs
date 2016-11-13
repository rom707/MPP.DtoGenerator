using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class StringTypeEntity : TypeEntity
    {
        public StringTypeEntity()
        {
            Type = "string";
            Format = "string";
            SystemType = "System.String";
        }
    }
}
