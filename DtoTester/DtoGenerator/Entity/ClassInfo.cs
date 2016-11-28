using System.Collections.Generic;

namespace DtoGenerator.Entity
{
    public class ClassInfo
    {
        public string ClassName { get; set; }
        public List<ClassProperty> Properties { get; set; }
    }
}
