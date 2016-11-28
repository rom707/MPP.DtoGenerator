using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using DtoGenerator.Entity;
using System.IO;

namespace DtoGenerator.Reader
{
    public class JsonClassReader : IReader
    {
        public List<ClassInfo> Read(string filename)
        {
            List<ClassInfo> result = new List<ClassInfo>();
            using (StreamReader streamReader = new StreamReader(filename))
            {
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ClassList list = serializer.Deserialize<ClassList>(reader);
                    result = list.ClassDescriptions;
                }
            }
            return result;
        }
    }
   
}
