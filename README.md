# Comparation

__Comparation__ is tiny library for work with equality and ordering.

![Comparation](logo.svg)

## Installation

Install [NuGet package](https://www.nuget.org/packages/Comparation/) using Package Manager

```
Install-Package Comparation -Version 0.1.3
```

## Equality

With __Comparation__ you will be able to define custom equality in just few lines

```csharp
using System;
using System.Collections.Generic;
using Comparation;

IEqualityComparer<Version> equality = Equality.Of<Version>()
    .By(version => version.Major)
    .AndBy(version => version.Minor);

equality.Equals(new Version(2, 17, 4), new Version(2, 17, 5)); // true
equality.Equals(new Version(2, 19, 4), new Version(2, 17, 4)); // false, Minor components are different
```

This is useful when you need to override equality in your own way or define it for a library type that does not provide proper Equals and GetHashCode methods.

You also can pass equality into collections

```csharp
var equality = Equality.Of<string>().By(@string => @string.Length);

var pets = new HashSet<string>(equality);
pets.Add("Dog");
pets.Add("Cat"); // Bad day for Cat ;), pets already contain element with length 3
pets.Add("Turtle");

Console.WriteLine(string.Join(", ", pets)); // Dog, Turtle
```

And finally you can easily compare entire collections

```csharp
IEqualityComparer<IReadOnlyCollection<string>> equality = Equality.Of<string>().Collection();

var required = new[] {"engine", "body", "door", "door", "windshield"};
var inventory = new[] {"body", "door", "windshield", "engine"};
equality.Equals(required, inventory); // false - second door is missing

var deliveries = new[] {"body", "door", "engine", "windshield", "door"};
equality.Equals(required, deliveries); // true
```

## Order

Order is defined very similar to equality

```csharp
IComparer<string> order = Order.Of<string>()
    .By(@string => @string[0])
    .ThenBy(@string => @string.Length);

order.Compare("Apple", "Banana"); // -1 (Apple less than Banana) by first letter
order.Compare("Brown", "Bohr"); // 1 (Brown greater than Bohr) by length since first letters are same
order.Compare("Cat", "Can"); // 0 (Cat equal to Can) by first letter and length
```

Order is useful when you need to customize sorting criteria at run time.

Do you want to reverse order? Easy - use `.Invert()`

```csharp
var ascendingOrder = Comparer<int>.Default;
var descendingOrder = ascendingOrder.Invert();

var numbers = new List<int> {7, 9, 16, 3};
numbers.Sort(descendingOrder); // 16, 9, 7, 3
```

With order you can compare sequences like this

```csharp
IComparer<IEnumerable<int>> order = Order.Of<int>().Sequence();

var myLuckyNumbers = new[] {1, 7, 32, 14, 4};
var lotteryNumbers = new[] {1, 7, 32, 28, 4};
order.Compare(myNumbers, lotteryNumbers); // -1 (14 is less than 28)
order.Compare(new[] {1, 2, 3}, new[] {1, 2}); // 1 sequences match by prefix, but first is longer
```