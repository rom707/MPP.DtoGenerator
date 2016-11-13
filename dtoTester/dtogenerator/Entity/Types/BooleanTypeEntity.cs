using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class BooleanTypeEntity : TypeEntity
    {
        public BooleanTypeEntity()
        {
            Type = "boolean";
            Format = String.Empty;
            SystemType = "System.Boolean";
        }
    }
}
