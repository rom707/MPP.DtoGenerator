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

        private JsonClassReader JsonReader = new JsonClassReader();

        private ReaderFactory() { }

        public static ReaderFactory GetInstance()
        {
            return Instance;
        }

        public JsonClassReader GetJsonReader()
        {
            return JsonReader;
        }
    }
}
