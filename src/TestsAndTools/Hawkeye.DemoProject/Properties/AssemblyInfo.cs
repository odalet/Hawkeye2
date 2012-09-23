// Hawkeye is licensed under the terms of the Microsoft Reciprocal License (Ms-RL)

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DOTNET4
[assembly: AssemblyTitle("Hawkeye.DemoProject.N4")]
#else
[assembly: AssemblyTitle("Hawkeye.DemoProject.N2")]
#endif

[assembly: AssemblyDescription("Hawkeye 2")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("http://hawkeye.codeplex.com")]
#if DOTNET4
[assembly: AssemblyProduct("Hawkeye 2 Test Application - .NET 4")]
#else
[assembly: AssemblyProduct("Hawkeye 2 Test Application - .NET 2")]
#endif
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("Hawkeye")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("0.1.0.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]