using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Entity
{
    public class GeneratedClass
    {
        public string ClassName { get; set; }
        public CompilationUnitSyntax Syntax { get; set; }

        public GeneratedClass(string name, CompilationUnitSyntax syntax)
        {
            ClassName = name;
            Syntax = syntax;
        }
    }
}
