using System.Windows.Forms;

namespace Hawkeye.UI
{
    //TODO: prevent Hawkeye main form from eating WM_ENABLE. 
    //      When modal dialogs are shownn by hawkeye itself,
    //      we want it to react normally (ie be disabled).

    /// <summary>Shows an Error dialog box.</summary>
    public static class ErrorBox
    {
        /// <summary>Shows an Error dialog box.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(string text)
        {
            return Show(null, text);
        }

        /// <summary>Shows an Error dialog box.</summary>
        /// <param name="owner">Dialog box top-level window and owner.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return MsgBox.Show(owner, text, SR.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>Shows a Warning dialog box.</summary>
    public static class WarningBox
    {
        /// <summary>Shows a Warning dialog box.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(string text)
        {
            return Show(null, text);
        }

        /// <summary>Shows a Warning dialog box.</summary>
        /// <param name="owner">Dialog box top-level window and owner.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return MsgBox.Show(owner, text, SR.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>Shows an Information dialog box.</summary>
    public static class InformationBox
    {
        /// <summary>Shows an Information dialog box.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(string text)
        {
            return Show(null, text);
        }

        /// <summary>Shows an Information dialog box.</summary>
        /// <param name="owner">Dialog box top-level window and owner.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Always returns <see cref="System.Windows.Forms.DialogResult.OK"/>.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return MsgBox.Show(owner, text, SR.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>Shows a Question dialog box.</summary>
    public static class QuestionBox
    {
        /// <summary>Shows a Question dialog box.</summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Returns either <see cref="System.Windows.Forms.DialogResult.Yes"/> or 
        /// <see cref="System.Windows.Forms.DialogResult.No"/> depending on the button which 
        /// was clicked by the user.</returns>
        public static DialogResult Show(string text)
        {
            return Show(null, text);
        }

        /// <summary>Shows a Question dialog box.</summary>
        /// <param name="owner">Dialog box top-level window and owner.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <returns>Returns either <see cref="System.Windows.Forms.DialogResult.Yes"/> or 
        /// <see cref="System.Windows.Forms.DialogResult.No"/> depending on the button which 
        /// was clicked by the user.</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return MsgBox.Show(owner, text, SR.Question, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }

    /// <summary>
    /// Wraps a call to either <see cref="System.Windows.Forms.MessageBox"/>
    /// or <see cref="ISimpleUIService.ShowMessageBox(string)"/>.
    /// </summary>
    internal static class MsgBox
    {
        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns></returns>
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            // Determine the correct options depending on the RTL property
            // of the current culture.
            MessageBoxOptions options = 0;
            if (System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
                options = MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;

            return MessageBox.Show(
                owner, text, caption, buttons, icon,
                MessageBoxDefaultButton.Button1, options);
        }
    }
}
