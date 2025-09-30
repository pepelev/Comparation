#if NETSTANDARD2_0
namespace Comparation
{
    internal struct HashCode
    {
        private int value;

        public void Add(int hashCode) => value = unchecked(value * 397) ^ hashCode;

        public int ToHashCode() => value;
    }
}
#endif