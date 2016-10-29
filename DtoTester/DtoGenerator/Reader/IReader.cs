using System.Collections.Generic;
using DtoGenerator.Entity;

namespace DtoGenerator.Reader
{
    public interface IReader
    {
        List<ClassInfo> Read(string classes);
    }
}
