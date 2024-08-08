# Nancy.Expressions
## _Towards a Computer Algebra System for Deterministic Network Calculus_

Nancy.Expressions is a C# library which provides the possibility of working on arbitrarily complex DNC symbolic expressions. 
It allows to effectively build expressions, flexibly visualizing them and evaluating them. 
The library is able to analyze an expression and apply equivalences to sub-parts of it before making any computation, possibly reducing its complexity.

## Language and requirements

Nancy.Expressions is a .NET 8.0 library, written in C# 12.
Both SDK and runtime for .NET are cross-platform, and can be downloaded from [here](https://dotnet.microsoft.com/en-us/download).

### DNC Equivalence Grammar - Antlr 4

To work with the grammar defined for parsing DNC equivalences:

1) Install the .NET Core C# ANTLR 4 runtime from NuGet:
- Antlr4.Runtime.Standard https://www.nuget.org/packages/Antlr4.Runtime.Standard

2) Install the ANTLR v4 plugin to support grammar development and C# code auto-generation of lexer and parser:
- For JetBrains Rider IDE: https://plugins.jetbrains.com/plugin/7358-antlr-v4 