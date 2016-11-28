using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity
{
    public class TypeEntity
    {
        public string Type { get; set; }
        public string Format { get; set; }

        public TypeEntity(string type, string format)
        {
            Type = type;
            Format = format;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (this is TypeEntity)
            {
                TypeEntity entity = (TypeEntity)obj;
                if (entity.Type == Type && entity.Format == Format)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 31 * Type.GetHashCode() + Format.GetHashCode();
        }
    }
}
