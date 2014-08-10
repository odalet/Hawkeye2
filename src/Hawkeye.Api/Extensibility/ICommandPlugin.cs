using System;
using System.Drawing;

namespace Hawkeye.Extensibility
{
    /// <summary>
    /// Specific plugin interface for plugins that provide an action 
    /// accessible through a menu or a toolbar button.
    /// </summary>
    public interface ICommandPlugin : IPlugin
    {
        /// <summary>
        /// Gets the label displayed on the menu (or toolbar button) for this command.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the image displayed on the menu (or toolbar button) for this command.
        /// </summary>
        Bitmap Image { get; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise, <c>false</c>.</returns>
        bool CanExecute();

        /// <summary>
        /// Occurs when the value returned by <see cref="CanExecute"/> changes.
        /// </summary>
        event EventHandler CanExecuteChanged;
    }
}
