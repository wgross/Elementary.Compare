# Elementary.Compare

Elementary.Compare applies the graph traversal agorithms from Elementary.Hierarchy to flatten a .Net object tree along the C# properties of these objects to a stream of key value pairs where the key is the path to a leaf and the value of the leaf. This allows to compare two objects trees by value.

## Installation

Add the nuget pacgae to a project using Visual Studio or the dot net commandline

```
dotnet add package Elementary.Compare
```
## Usage

To flatten an object tree to instances of KeyValue<string,object> where the 'Key' is a path to a leaf property and 'Value' is the leaf properties value as object instance:

```C#
using Elementary.Compare;

var obj = new {
  a = new {
    b = 1
  }
};

var result = obj.Flatten().ToArray();

Assert.Equal("a/b", result.Single().Key);
Assert.Equal(1, result.Single().Value);
```

To verify equality of two identically structured object trees.

```C#
using Elementary.Compare;

var left = new {
  ...
};

var right = new {
  ...
};

Assert.True(left.DeepEquals(right));
```

To compare to instance of object trees and get a report of their differences and equalities:

```C#

using Elementary.Compare;

var left = new {
  ...
};

var right = new {
  ...
};

var result = left.DeepCompare(left);

Assert.False(result.AreEqual)
Assert.Equal(2, result.EqualValues.Count());
Assert.Contains("a/b", result.Missing.Right); // right is missing property left.a.b
Assert.Contains("c/d", result.Missing.Left); // left is missing property right.c.d
Assert.Contains("x/y", result.Different.Values); // left and right have property .x.y, but values differ
Assert.Contains("o/p", result.Different.Types); // left and right have property .o.p, but property types differ
```

To create property pathes in a refactorable way:

```C#

using Elementary.Compare;

var obj = {
  a = "txt"
  b = new[]{ 1 }
};

Assert.Equal(HierarchyPath.Create("a"), obj.PropertyPath(o => o.a));
Assert.Equal(HierarchyPath.Create("b","0"), obj.PropertyPath(o => o.b[0]));
```
