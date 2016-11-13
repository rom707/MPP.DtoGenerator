using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Type
{
    public class Int32TypeEntity : TypeEntity
    {
        public Int32TypeEntity() 
        {
            Type = "integer";
            Format = "int32";
            SystemType = "System.Int32";
        }
    }
}
