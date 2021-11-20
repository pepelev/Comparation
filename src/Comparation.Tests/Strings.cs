using System;
using System.Collections.Generic;
using System.Linq;

namespace Comparation.Tests
{
    public static class Strings
    {
        public static IReadOnlyList<string> NonNull { get; } = new[]
        {
            "Hello",
            "world",
            "positive",
            "negative",
            "dog",
            "Dog",
            "cat",
            "fox",
            "quick",
            string.Concat("Qui", "ck"),
            "Firmware",
            "Yeti",
            "Bigfoot",
            "Brown",
            "Blast",
            "beast",
            "Beast",
            "Apple",
            "Alpine",
            "Fear",
            "Calm",
            "Friends",
            "Believe"
        };

        private static IEnumerable<string?> NonNullNullable => NonNull;
        public static IEnumerable<string?> All => NonNullNullable.Append(null);
        public static IEnumerable<IReadOnlyCollection<string?>> Collections => Pick(All);
        public static IEnumerable<IReadOnlyCollection<string>> NonNullableCollections => Pick(NonNull);

        private static IEnumerable<IReadOnlyCollection<T>> Pick<T>(IEnumerable<T> sequence)
        {
            var all = sequence.ToList();
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var size = random.Next(0, 16);
                var collection = new List<T>();
                for (var j = 0; j < size; j++)
                {
                    collection.Add(
                        all[random.Next(all.Count)]
                    );
                }

                yield return collection;
            }
        }
    }
}