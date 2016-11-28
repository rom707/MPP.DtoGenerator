using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DtoGenerator.Entity;
using DtoGenerator.ThreadPool;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;

namespace DtoGenerator
{
    public class Generator : IDisposable
    {

        private delegate void ThreadFinishCallback();

        private CountdownEvent _countdown;
        private object _lockGeneratedClassesObject = new object();
        private CustomThreadPool _threadPool;

        private List<GeneratedClass> _generatedClasses = new List<GeneratedClass>();
        private Queue<InnerThreadPoolHelper> _waitingQueue = new Queue<InnerThreadPoolHelper>();

        private int _countTasks = 3;
        private int _currentTaskInThreadPool = 0;

        private class InnerThreadPoolHelper
        {
            internal ClassInfo ClassInfo { get; set; }
            internal ThreadFinishCallback FinallyCallback { get; set; }
        }

        public Generator(int countTasks)
        {
            try
            {
                _threadPool = new CustomThreadPool(countTasks);
                _countTasks = countTasks;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                _threadPool = new CustomThreadPool(_countTasks);
            }
        }

        public List<GeneratedClass> GenerateClasses(List<ClassInfo> classList)
        {
            _countdown = new CountdownEvent(classList.Count);
            foreach (var ClassInfo in classList)
            {
                var innerThreadPoolHelper = new InnerThreadPoolHelper()
                {
                    ClassInfo = ClassInfo,
                    FinallyCallback = () =>
                    {
                        lock (_waitingQueue)
                        {
                            _currentTaskInThreadPool--;
                            if (_waitingQueue.Count > 0)
                            {
                                AddTaskToThreadPool(_waitingQueue.Dequeue());
                            }
                        }
                        _countdown.Signal();
                    }
                };
                AddTaskToThreadPool(innerThreadPoolHelper);
            }
            _countdown.Wait();
            return _generatedClasses;
        }

        private void AddTaskToThreadPool(InnerThreadPoolHelper task)
        {
            lock (_waitingQueue)
            {
                if (_currentTaskInThreadPool < _countTasks)
                {
                    _currentTaskInThreadPool++;
                    _threadPool.QueueUserWorkItem(new WaitCallback(GenerateCallBack), task);
                }
                else
                {
                    _waitingQueue.Enqueue(task);
                }
            }
        }

        private void GenerateCallBack(object state)
        {
            Console.WriteLine($"Current thread id = {Thread.CurrentThread.ManagedThreadId}");

            var item = (InnerThreadPoolHelper)state;

            try
            {
                try
                {
                    GeneratedClass generatedClass = GenerateClass(item.ClassInfo);
                    lock (_lockGeneratedClassesObject)
                    {
                        _generatedClasses.Add(generatedClass);
                    }
                }
                catch (ArgumentException exeption)
                {
                    Console.WriteLine(exeption.Message);
                    Console.WriteLine($"Class {item.ClassInfo.ClassName} will not be create");
                }
            }
            finally
            {
                item.FinallyCallback();
            }
        }


        private TypeSyntax GenerateType(string type, string format)
        {
            TypeContainer container = TypeContainer.GetInstanse();
            string SystemType = container.GetSystemType(new TypeEntity(type, format));
            return SyntaxFactory.ParseTypeName(SystemType);
        }

        public PropertyDeclarationSyntax GenerateProperty(ClassProperty field)
        {
            TypeSyntax type = GenerateType(field.Type, field.Format);
            PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(type, field.Name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
            return property;
        }

        public PropertyDeclarationSyntax[] GenerateProperties(List<ClassProperty> propertiesInfo)
        {
            PropertyDeclarationSyntax[] fields;
            if (propertiesInfo != null)
            {
                fields = new PropertyDeclarationSyntax[propertiesInfo.Count];
                int i = 0;
                foreach (var item in propertiesInfo)
                {
                    fields[i] = GenerateProperty(item);
                    i++;
                }
            }
            else
            {
                fields = new PropertyDeclarationSyntax[0];
            }
            return fields;
        }

        public GeneratedClass GenerateClass(ClassInfo classInfo)
        {
            ClassDeclarationSyntax classDeclarationSyntax = SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(classInfo.ClassName))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(GenerateProperties(classInfo.Properties));

            CompilationUnitSyntax compilationUnitSyntax = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")))
                .AddMembers(classDeclarationSyntax);
            return new GeneratedClass(classInfo.ClassName, compilationUnitSyntax);
        }

        public void Dispose()
        {
            _countdown.Dispose();
            _threadPool.Dispose();
        }
    }
}
