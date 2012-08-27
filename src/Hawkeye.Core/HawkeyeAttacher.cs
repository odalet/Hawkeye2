// This namespace will hopefully never be used elsewhere. This is to prevent accidental
// use of methods part of the 'Hawkeye' namespace that may be defined in the Hawkeye.Api assembly.
// For the same reason, when coding, we don't resolve unknown types by adding a 'using namespace' 
// declaration, but we intentionally use fully explicit types names. Even 'using System' is forbidden
// Because LinqBridge (defined in Hawkeye.Api) adds types (Func and Action for example) to the System
// namespace.
namespace __HawkeyeAttacherNamespace__
{
    /// <summary>
    /// Allows native c++ code to attach Hawkeye to a running process
    /// </summary>
    internal static class HawkeyeAttacher
    {
        // This class must be the simplest possible in its interface; 
        // it should not reference any type outside its own assembly (and the fx ones)
        // otherwise, the c++ code will be unable to load the type by reflection:
        // it will fail trying to resolve assemblies containing referenced types as 
        // these assemblies (Hawkeye.Api.dll for instance) can't be found in the process
        // assemblies search path: remember that now the running process is not Hawkeye.exe
        // any more but the spied application.

        /// <summary>
        /// Attaches the (injected) Hawkeye application to the specified target window
        /// (and destroys the original Hawkeye window).
        /// </summary>
        /// <param name="targetHandle">The target window.</param>
        /// <param name="hawkeyeHandle">The original Hawkeye window.</param>
        private static void Attach(System.IntPtr targetHandle, System.IntPtr hawkeyeHandle)
        {
            // Because when injected the base probing path is the one 
            // of the injected app, not the hawkeye root path.
            System.AppDomain.CurrentDomain.AssemblyResolve += (s, r) =>
                Hawkeye.Reflection.AssemblyResolver.Resolve(r.Name);
            CallAttach(targetHandle, hawkeyeHandle);
        }

        // We need this method to 'hide' the Hawkeye.HawkeyeApplication type from the c++ code
        // Once this code is called, the Hawkeye.Api assembly can be resolved thanks to the assembly 
        // resolver we've setup in the Attach method.
        // TODO: Hope this call won't be inlined by the 'release' optimizer.
        private static void CallAttach(System.IntPtr targetHandle, System.IntPtr hawkeyeHandle)
        {
            Hawkeye.HawkeyeApplication.Attach(targetHandle, hawkeyeHandle);
        }

        private static System.IntPtr StringToIntPtr(string ptr)
        {
            return System.IntPtr.Size == 8 ? 
                new System.IntPtr(long.Parse(ptr)) : new System.IntPtr(int.Parse(ptr));
        }
    }
}
