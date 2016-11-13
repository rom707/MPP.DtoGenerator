using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity.Types
{
    class DateTypeEntity : TypeEntity
    {
        public DateTypeEntity()
        {
            Type = "string";
            Format = "date";
            SystemType = "System.DateTime";
        }
    }
}
