# About

Library for analytical calculation of mathematical expressions. 
Сurrently derivative and simplifiers are supported.

P.S. this is just my project to learn some C# basics, so take it easy

# Notes

1. **For all of this to work you need to prepend each piece with**
    ```
    using static Derivas.Expression.DvOps;
    ```
    But you may use it as you want in your real code.
    
2. Most of the methods accept `object` as it's argument, but
    this means that you can pass in numeric type(shortcut for a `Const`) or
    string(for a `Sym`) and `IDvExpr`. This was done to prevent writing this:
    ```
    Add(Const(3), Sym("x"), Pow(Const(5), Const(3))
    ```
    And transform it to 
    
    ```
    Add(3, "x", Pow(5, 3))
    ```
    
# Table of contents

- [About](#about)
- [Notes](#notes)
- [Table of contents](#table-of-contents)
- [Example](#example)
- [Entities](#entities)
    + [`IDvExpr`](#idvexpr)
    + [`IDvSimplifier`](#idvsimplifier)
      - [ByConst](#byconst)
      - [ByPolynom](#bypolynom)
      - [ByPartial](#bypartial)
      - [ByCustom](#bycustom)
- [Expressions reference](#expressions-reference)
    + [CommutativeAssocitaiveOperator](#commutativeassocitaiveoperator)
    + [BinaryOperator](#binaryoperator)
    + [UnaryOperator](#unaryoperator)
    + [Others](#others)
- [Utility](#utility)
    + [Dict](#dict)
    + [Constants](#constants)

# Example

```
using static Derivas.Expression.DvOps;

namespace Usage
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var finExpr = Add(Pow(Sin("x"), 2), Pow(Cos("x"), 2));
            var finDict = new Dictionary<string, double>() 
            { 
                { "x", Math.PI }, { "y", 1 }
            };
            Print(finExpr, finDict);
            // sin(x) ^ 2 + cos(x) ^ 2
            // 1
            var derived = Der(finExpr, "x");
            Print(derived, finDict);
            // 2 * sin(x) * cos(x) - 2 * cos(x) * sin(x)
            // 0
            
            var simplified = Simpl(finExpr)
                .ByCustom(Pow(Sin("x"), 2), 3)
                .Simplify();
            Print(simplified, finDict);
            // 3 + cos(x) ^ 2
            // 4
        }

        private static void Print(IDvExpr expr, Dictionary<string, double> dict)
        {
            Console.WriteLine(expr.Represent());
            Console.WriteLine(expr.Calculate(dict));
        }
    }
}
```

# Entities

There are two main entities in Derivas: `Expression` and `Simplifier`. Which implemented as interfaces: `IDvExpr` and `IDvSimplifier` accordingly.

### `IDvExpr`

Ultimate base interface. Symbol, Constant, Operators implement it.

```
interface IDvExpr : IEquatable<IDvExpr>
{
    double Calculate(IDictionary<string, double> concrete);
    string Represent();
}
```

Go to the full expressions reference [here](#expressions-reference).

### `IDvSimplifier`

Simplifier performs some kind of transformation, it has only one method:
```
IDvExpr Simplify(IDvExpr expr)
```

In external API you should use it like this: 
```
IDvExpr res = Simpl(/* IDvExpr */ expr)
    .By...()
    .By...()
    .Simplify();
```
There are 4 kinds of simplifiers:

#### ByConst

Partially reduce constants in operators:
```
Add(3, 5, 10) -> Const(18)
Mul(3, 5, "x") -> Add(8, "x")
```

#### ByPolynom

Collect similiar expressions in one scope:
```
Add("x", "x", "x") -> Mul(3, "x")
Add("x", "y", "x") -> Add(Mul(2, "x"), "y")
```

#### ByPartial

Partially replace some symbols from dictionary or by hand:
The `Dict` is explained [here](#utility)
```
IDictionary<string, double> d = Dict.Add("x", 5).Add("y", 3).Get();
var expr = Add("x", Pow(2, "y"), "z");
var simplified = Simpl(expr).ByPartial(d).Simplify();
// 5 + 2 ^ 3 + z
var orTheSame = Simpl(expr).ByPartial("x", 5).ByPartial("y", 3).Simplify();
// 5 + 2 ^ 3 + z
```

#### ByCustom

Replace one IDvExpr with other or use your own simplfiers:
```
var expr = Log(Mul(2, "x"), 3);

var replaced = Simpl(expr).ByCustom(3, DvConsts.E).Simplify();
var orTheSame = Simpl(expr)
    .ByCustom(expr => Const(3).Equals(expr) ? DvConsts.E : expr)
    .Simplify();
```

# Expressions reference

### CommutativeAssocitaiveOperator

* API: `Add`, `Mul`
* Description: takes more than one argument, order doesn't matter
* Example: `Add(3, 5, "x", Add(6, 3))`
* Throws: `DvNotEnoughArguments`

### BinaryOperator

* API: `Sub`, `Div`, `Pow`
* Description: takes only two arguments, order does matter
* Example: `Div(Pow(1, 2), 0)`
* Throws: **no Dv exceptions**

### UnaryOperator

* API:
    1. `Cos`, `Sin`, `Tan`, `Cotan`
    2. `Acos`, `Asin`, `Atan`, `Acotan`
    3. `Cosh`, `Sinh`, `Tanh`, `Cotanh`
* Description: takes one argument
* Example: `Cos(Sin(DvConsts.PI)) // equals 1` 
* Throws: **no Dv exceptions**

### Others

* `Log` - acts just like `Math.Log`, takes two parameters and one is optional
* `Der` - takes expression and symbol to take derivative on. 

# Utility

Some useful features to help you using the library.

### Dict

You will frequently need an `IDictionary<string, double>` to pass into `Calculate` or other such methods. but it's very verbose to create new dictionary each time, so here you go, a shortcut:
```
double res = expr.Calculate(Dict.Add("x", 1).Add("y", 3).Get());
```

### Constants

Common mathematical constants in `IDvExpr`(surrounded with `DvOps.Const`):
* `DvOps.DvConsts.E`
* `Dvops.DvConsts.PI`
