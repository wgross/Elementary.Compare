# Elementary.Compare

Elementary.Compare provides some algorithms to compare two object graphs of different class types by (property) value. Bothy object graphs are traversed and for all public properties thet postion (paths of public properties leading to them) and their values are compared. The result is a description of differences (value and value type) and missing properties on each side (paths which doesn't exist on both sides). 

## Installation

Add the nuget packgae to a project using Visual Studio or the dot net commandline

```
dotnet add package Elementary.Compare
```
## Usage

To compare two classes use the Diff extension method:

```C#
using Elementary.Compare;

var left = new {
  ...
};

var right = new {
  ...
};

var diffResult = left.Diff(right));

Assert.True(diffResult.AreEqual)
```

To compare the state of a class before and after a change, use a checkpoint:

```C#
using Elementary.Compare;

var obj = new Data{
  a = new SubData {
    b = 1
  }
};

var checkpoint = obj.Flatten().Build();

// change the original object
obj.a.b = 2

var diffResult = obj.Diff(checkpoint);

Assert.False(diffResult.AreEqual);
Assert.Equal(PropertyPath.Make(obj, o => o.a.b), diffResult.Different.Values.Single());
```

If working with test data as input of test scenarios (dto-mapping for example) you might want to be sure that all properties are filled with a non-default value. To verify this use the NoPropertyHasDefaultValue extension method. It will reject empty collections, properties with value of default(T) and null for Nullable<T> Properties or reference type properties.
  
```C#
using Elementary.Compare;

var obj = new  {
 ...
};

var result = obj.NoPropertyHasDefaultValue();

Assert.True(result);
```
 
