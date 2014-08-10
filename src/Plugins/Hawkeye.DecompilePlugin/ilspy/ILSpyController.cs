using System;
using System.Text;

namespace Hawkeye.DecompilePlugin
{
    // Based on information found here: https://github.com/icsharpcode/ILSpy/blob/master/doc/Command%20Line.txt:
    // ILSpy Command Line Arguments
    // 
    // Command line arguments can be either options or file names.
    // If an argument is a file name, the file will be opened as assembly and added to the current assembly list.
    //
    // Available options:
    //     /singleInstance    If ILSpy is already running, activates the existing instance
    //                        and passes command line arguments to that instance.
    //                        This is the default value if /list is not used.
    //     
    //     /separate          Start up a separate ILSpy instance even if it is already running.
    //     
    //     /noActivate        Do not activate the existing ILSpy instance. This option has no effect
    //                        if a new ILSpy instance is being started.
    //     
    //     /list:listname     Specifies the name of the assembly list that is loaded initially.
    //                        When this option is not specified, ILSpy loads the previously opened list.
    //                        Specify "/list" (without value) to open the default list.
    //                        
    //                        When this option is used, ILSpy will activate an existing instance
    //                        only if it uses the same list as specified.
    //                        
    //                        [Note: Assembly Lists are not yet implemented]
    //     
    //     /clearList         Clears the assembly list before loading the specified assemblies.
    //                        [Note: Assembly Lists are not yet implemented]
    //     
    //     /navigateTo:tag    Navigates to the member specified by the given ID string.
    //                        The member is searched for only in the assemblies specified on the command line.
    //                        Example: 'ILSpy ILSpy.exe /navigateTo:T:ICSharpCode.ILSpy.CommandLineArguments'
    //                        
    //                        The syntax of ID strings is described in appendix A of the C# language specification.
    //     
    //     /language:name     Selects the specified language.
    //                        Example: 'ILSpy /language:C#' or 'ILSpy /language:IL'
    //
    // WM_COPYDATA (SendMessage API):
    // ILSpy can be controlled by other programs that send a WM_COPYDATA message to its main window.
    // The message data must be an Unicode (UTF-16) string starting with "ILSpy:\r\n".
    // All lines except the first ("ILSpy:") in that string are handled as command-line arguments.
    // There must be exactly one argument per line.
    //
    // That is, by sending this message:
    //     ILSpy:
    //     C:\Assembly.dll
    //     /navigateTo:T:Type
    // The target ILSpy instance will open C:\Assembly.dll and navigate to the specified type.
    //
    // ILSpy will return TRUE (1) if it handles the message, and FALSE (0) otherwise.
    // The /separate option will be ignored; WM_COPYDATA will never start up a new instance.
    // The /noActivate option has no effect, sending WM_COPYDATA will never activate the window.
    // Instead, the calling process should use SetForegroundWindow().
    // If you use /list with WM_COPYDATA, you need to specify /singleInstance as well, otherwise
    // ILSpy will not handle the message if it has opened a different assembly list.
    internal class ILSpyController : BaseDecompilerController
    {
        /// <summary>
        /// Gets a value indicating whether a decompiler instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if a running decompiler instance could be found; otherwise, <c>false</c>.
        /// </value>
        public override bool IsRunning
        {
            get { return TargetWindow != IntPtr.Zero; }
        }

        protected override bool DoesWindowTitleMatches(string title)
        {
            if (string.IsNullOrEmpty(title)) return false;
            return title.StartsWith("ILSpy");
        }

        /// <summary>
        /// Loads the type's assembly then selects the specified type declaration in the decompiler;
        /// </summary>
        /// <param name="type">The type to decompile.</param>
        /// <returns>
        ///   <c>true</c> if the action succeeded; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool GotoType(Type type)
        {
            const string template = "ILSpy:\r\n{0}\r\n/navigateTo:T:{1}\r\n";
            var assemblyFileName = type.Module.FullyQualifiedName;
            return base.Send(string.Format(template, assemblyFileName, type.FullName));
        }
    }
}
