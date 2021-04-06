using System;
using System.Linq;

namespace Comparation.Tests.Core
{
    public sealed class NamedByType<T> : Named<T> where T : class
    {
        private readonly string name;

        public NamedByType(string name, T value)
        {
            this.name = name;
            Value = value;
        }

        public override T Value { get; }
        public override string Name => $"{TypeName}({name})";
        private string TypeName => PrettyName(Value.GetType());

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