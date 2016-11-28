using DtoGenerator.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.CodeSaver
{
    public class CodeSaver
    {

        private string _pathToSave;
        public CodeSaver(String path)
        {
            _pathToSave = path;
        }

        public void Save(List<GeneratedClass> classes)
        {
            foreach (var item in classes) {
                using (TextWriter writer = File.CreateText(_pathToSave + item.ClassName + ".cs"))
                {
                    writer.Write(GenerateCodeString(item.Syntax));
                }
            }
        }

        public string GenerateCodeString(CompilationUnitSyntax syntax)
        {
            SyntaxNode formattedNode = Formatter.Format(syntax, MSBuildWorkspace.Create());
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                formattedNode.WriteTo(writer);
            }
            return sb.ToString();
        }

    }
}
