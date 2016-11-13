using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class FloatTypeEntity : TypeEntity
    {
        public FloatTypeEntity()
        {
            Type = "number";
            Format = "float";
            SystemType = "System.Single";
        }
    }
}
