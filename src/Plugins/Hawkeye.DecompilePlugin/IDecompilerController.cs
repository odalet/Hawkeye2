using System;

namespace Hawkeye.DecompilePlugin
{
    internal interface IDecompilerController
    {
        /// <summary>
        /// Gets a value indicating whether a decompiler instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a running decompiler instance could be found; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// Loads the type's assembly then selects the specified type declaration in the decompiler; 
        /// </summary>
        /// <param name="type">The type to decompile.</param>
        /// <returns><c>true</c> if the action succeeded; otherwise, <c>false</c>.</returns>
        bool GotoType(Type type);
    }
}
