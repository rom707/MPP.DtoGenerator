using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using DtoGenerator.Entity;

namespace DtoGenerator.Reader
{
    public class JsonReader : IReader
    {
        public List<ClassInfo> Read(string classes)
        {
            if (classes == null)
            {
                throw new ArgumentNullException();
            } 
            return JsonConvert.DeserializeObject<List<ClassInfo>>(classes);
        }
    }
}
