using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity
{
    public class TypeContainer
    {
        private static TypeContainer instanse = null;
        private Dictionary<TypeEntity, string> container;
        private static object lockObj = new object();

        public TypeContainer()
        {
             container = new Dictionary<TypeEntity, string>()
             {
                 { new TypeEntity("integer", "int32"), "System.Int32" },
                 { new TypeEntity("integer", "int64"), "System.Int64" },
                 { new TypeEntity("number", "float"), "System.Single" },
                 { new TypeEntity("number", "double"), "System.Double" },
                 { new TypeEntity("string", "byte"), "System.Byte" },
                 { new TypeEntity("boolean", ""), "System.Boolean" },
                 { new TypeEntity("string", "string"), "System.String" },
                 { new TypeEntity("string", "date"), "System.DateTime" }
             };
        }

        public static TypeContainer GetInstanse()
        {
            if(instanse == null)
            {
                lock (lockObj)
                {
                    if(instanse == null)
                    {
                        instanse = new TypeContainer();
                    }
                }
            }
            return instanse;
        }

        public string GetSystemType(TypeEntity entity)
        {
            if (container.ContainsKey(entity))
            {
                return container[entity];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
