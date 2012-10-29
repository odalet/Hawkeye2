using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Hawkeye.UI
{
    internal class GenericComponentEditor : WindowsFormsComponentEditor
    {
        /// <summary>
        /// Gets the component editor pages associated with the component editor.
        /// </summary>
        /// <returns>
        /// An array of component editor pages.
        /// </returns>
        protected override Type[] GetComponentEditorPages()
        {
            return new Type[] 
            {
                typeof(GenericComponentEditorPage), 
                typeof(GenericComponentEditorPage2),
                typeof(GenericComponentEditorPage3) 
            };
        }

        /// <summary>
        /// Creates an editor window that allows the user to edit the specified component.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain additional context information.</param>
        /// <param name="component">The component to edit.</param>
        /// <param name="owner">An <see cref="T:System.Windows.Forms.IWin32Window" /> that the component belongs to.</param>
        /// <returns>
        /// true if the component was changed during editing; otherwise, false.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
        {
            var componentEditorPages = GetComponentEditorPages();
            if (componentEditorPages == null || componentEditorPages.Length == 0)
                return false;

            using (var form = CreateForm(component, componentEditorPages)) return form.ShowForm(
                owner, GetInitialComponentEditorPageIndex()) == DialogResult.OK;
        }

        private ComponentEditorForm CreateForm(object component, Type[] pages)
        {
            var form = new ComponentEditorForm(component, pages);
            form.ShowIcon = false;
            return form;
        }
    }
}
