using System;

namespace Hawkeye.DecompilePlugin
{
    internal class ReflectorController : BaseDecompilerController
    {
        #region API
        
        /// <summary>
        /// Gets a value indicating whether a decompiler instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if a running decompiler instance could be found; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool IsRunning
        {
            get { return base.Send("Available\n4.0.0.0"); }
        }

        /// <summary>
        /// Loads the type's assembly then selects the specified type declaration in the decompiler;
        /// </summary>
        /// <param name="type">The type to decompile.</param>
        /// <returns>
        ///   <c>true</c> if the action succeeded; otherwise, <c>false</c>.
        /// </returns>
        public override bool GotoType(Type type)
        {
            // Loads the assembly
            var assemblyFileName = type.Module.FullyQualifiedName;
            base.Send("LoadAssembly\n" + assemblyFileName);

            // Select the type
            return base.Send("SelectTypeDeclaration\n" + type.FullName);
        }

        #endregion

        #region Other Reflector API Calls (for reference purpose)

        private bool SelectTypeDeclaration(Type type)
        {
            return Send("SelectTypeDeclaration\n" + type.FullName);
        }

        private bool LoadAssembly(string filename)
        {
            return base.Send("LoadAssembly\n" + filename);
        }

        private bool SelectEventDeclaration(string key)
        {
            return Send("SelectEventDeclaration\n" + key);
        }

        private bool SelectFieldDeclaration(string key)
        {
            return Send("SelectFieldDeclaration\n" + key);
        }

        private bool SelectMethodDeclaration(string key)
        {
            return Send("SelectMethodDeclaration\n" + key);
        }

        private bool SelectPropertyDeclaration(string key)
        {
            return Send("SelectPropertyDeclaration\n" + key);
        }

        private bool SelectTypeDeclaration(string key)
        {
            return Send("SelectTypeDeclaration\n" + key);
        }

        private bool UnloadAssembly(string filename)
        {
            return Send("UnloadAssembly\n" + filename);
        }

        #endregion

        /// <summary>
        /// Determines wether the specified window title matches the actual decompiler.
        /// </summary>
        /// <param name="title">The window title.</param>
        /// <returns>
        ///   <c>true</c> if matches; otherwise, <c>false</c>.
        /// </returns>
        protected override bool DoesWindowTitleMatches(string title)
        {
            if (string.IsNullOrEmpty(title)) return false;

            // FIX: issue http://hawkeye.codeplex.com/workitem/7784
            // Reflector 6 and 7 (beta) titles start with ".NET Reflector"
            if (title.StartsWith(".NET Reflector"))
                return true;
            else if (title.StartsWith("Red Gate's .NET Reflector"))
                return true;
            else if (title.StartsWith("Lutz Roeder's .NET Reflector"))
                return true;

            return false;
        }
    }
}
