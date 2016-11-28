using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;
using DtoGenerator.Entity;
using DtoGenerator.Reader;
using DtoGenerator.Reader.Factory;
using System.IO;
using DtoGenerator.CodeSaver;

namespace DtoTester
{
    class Program
    {
        const int CountOfArguments = 2;

        static void Main(string[] args)
        {
            if (args.Length != CountOfArguments)
            {
                Console.WriteLine($"Count of arguments should be {CountOfArguments}");
                return;
            }

            DtoGenerate(args[0], args[1]);

            Console.ReadKey();
        }
        static void DtoGenerate(string pathToJson, string pathToOutputFilesDirectory)
        {
            try
            {
                ReaderFactory factory = ReaderFactory.GetInstance();
                IReader reader = factory.GetJsonReader();
                List<ClassInfo> classList = reader.Read(pathToJson);

                using (var generator = new Generator(CountOfArguments))
                {
                    new CodeSaver(pathToOutputFilesDirectory).Save(generator.GenerateClasses(classList));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
