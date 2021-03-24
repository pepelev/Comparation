using System;
using System.Linq;

namespace Comparation.Tests.Core
{
    public sealed class NamedByType : NamedTest
    {
        private readonly Test test;
        private readonly string name;

        public NamedByType(string name, Test test)
        {
            this.test = test;
            this.name = name;
        }

        public override void Run()
        {
            test.Run();
        }

        public override string Name => $"{TypeName}({name})";
        private string TypeName => PrettyName(test.GetType());

        private static string PrettyName(Type type)
        {
            if (type.GetGenericArguments().Length == 0)
            {
                return type.Name;
            }

            var genericArguments = type.GetGenericArguments();
            var typeDefinition = type.Name;
            var properName = typeDefinition.Substring(0, typeDefinition.IndexOf("`"));
            return $"{properName}<{string.Join(", ", genericArguments.Select(PrettyName))}>";
        }
    }
}