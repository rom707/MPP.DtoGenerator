using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Reader.Factory
{
    public class ReaderFactory
    {
        private static ReaderFactory Instance = new ReaderFactory();

        private JsonReader JsonReader = new JsonReader();

        private ReaderFactory() { }

        public static ReaderFactory GetInstance()
        {
            return Instance;
        }

        public JsonReader GetJsonReader()
        {
            return JsonReader;
        }
    }
}
