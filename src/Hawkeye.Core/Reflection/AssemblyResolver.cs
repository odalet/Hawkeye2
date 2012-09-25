using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Hawkeye.Reflection
{
    /// <summary>
    /// Specific assembly resolver used when hawkeye is injected into another process.
    /// </summary>
    internal static class AssemblyResolver
    {
        // We cache unresolved assemblies to speed up things a bit (for instance 
        // when looking for inexistant resource assemblies).
        private static List<string> unresolvedAssemblies = new List<string>();
        // Let's also cache resolved ones
        private static Dictionary<string, string> resolvedAssemblies = new Dictionary<string, string>();

        public static Assembly Resolve(string assemblyName)
        {
            // First try caches
            if (unresolvedAssemblies.Contains(assemblyName)) 
                return null;
            if (resolvedAssemblies.ContainsKey(assemblyName))
                return Assembly.LoadFrom(resolvedAssemblies[assemblyName]);

            // RequestingAssembly exists in .NET 4 but not before
            var requestingAssembly = Assembly.GetExecutingAssembly();
            var directory = Path.GetDirectoryName(requestingAssembly.Location);
            // see http://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net
            var file = Path.Combine(directory, new AssemblyName(assemblyName).Name + ".dll");
            
            if (!File.Exists(file))
            {
                unresolvedAssemblies.Add(assemblyName);
                return null; // will throw!
            }
            else
            {
                resolvedAssemblies.Add(assemblyName, file);
                return Assembly.LoadFrom(file);
            }
        }
    }
}
