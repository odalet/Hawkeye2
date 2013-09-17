using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Hawkeye.UI.Controls
{
    // Inspiration found in Hawkeye's search box extender
    internal class PropertyGridEx : PropertyGrid
    {
        private bool alreadyInitialized = false;
        private ToolStrip thisToolStrip = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGridEx"/> class.
        /// </summary>
        public PropertyGridEx()
        {
            base.ToolStripRenderer = new ToolStripProfessionalRenderer()
            {
                RoundedEdges = false
            };

            if (IsHandleCreated) Initialize();
            else
            {
                HandleCreated += (s, e) => Initialize();
                VisibleChanged += (s, e) => Initialize();
            }
        }
        
        protected ToolStrip ToolStrip
        {
            get
            {
                if (thisToolStrip == null)
                {
                    var field = typeof(PropertyGrid).GetField("toolStrip",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    thisToolStrip = (ToolStrip)field.GetValue(this);
                }

                return thisToolStrip;
            }
        }

        protected bool IsProcessingSelection
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            if (alreadyInitialized) return;
            if (ToolStrip == null) return;

            InitializeToolStrip();

            alreadyInitialized = true;
        }

        /// <summary>
        /// Initializes the tool strip.
        /// </summary>
        protected virtual void InitializeToolStrip()
        {
            ToolStrip.Items.Add(new ToolStripSeparator());
            ToolStrip.Items.AddRange(CreateToolStripItems());
        }

        /// <summary>
        /// Creates the tool strip items.
        /// </summary>
        /// <returns></returns>
        protected virtual ToolStripItem[] CreateToolStripItems()
        {
            return new ToolStripItem[] { };
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.PropertyGrid.SelectedObjectsChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            if (IsProcessingSelection)
            {
                base.OnSelectedObjectsChanged(e);
                return;
            }

            IsProcessingSelection = true;
            try
            {
                var selection = base.SelectedObjects;
                base.SelectedObjects = ProcessSelection(selection).ToArray();                
            }
            finally
            {
                IsProcessingSelection = false;
            }
        }

        /// <summary>
        /// Processes the current property grid <see cref="SelectedObjects"/>, giving a chance to alter it.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <returns>The altered selection</returns>
        protected virtual IEnumerable<object> ProcessSelection(IEnumerable<object> selection)
        {
            if (selection == null) return selection;
            return selection
                .Select(item => item.GetInnerObject())
                .Where(item => item != null);
        }
    }

    //// Inspiration found in Hawkeye's search box extender
    //internal partial class PropertyGridEx : PropertyGrid
    //{
    //    private object[] wrappedObjects = new object[] { };
    //    //private object selectedObject = null;
    //    //private object[] selectedObjects = null;

    //    private bool alreadyInitialized = false;
    //    private ToolStrip thisToolStrip = null;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="PropertyGridEx"/> class.
    //    /// </summary>
    //    public PropertyGridEx()
    //    {
    //        ProtectConnectionStrings = false;
    //        base.ToolStripRenderer = new ToolStripProfessionalRenderer()
    //        {
    //            RoundedEdges = false
    //        };

    //        if (IsHandleCreated) Initialize();
    //        else
    //        {
    //            HandleCreated += (s, e) => Initialize();
    //            VisibleChanged += (s, e) => Initialize();
    //        }

    //        base.SelectedObjectsChanged += (s, e) => OnSelectionChanged();
    //    }

    //    /// <summary>
    //    /// Gets or sets a value indicating whether to protect connection strings.
    //    /// </summary>
    //    /// <remarks>
    //    /// If set to <c>true</c>, we search for a property named <c>ConnectionString</c>
    //    /// in the selected objects, if it is found, then it is displayed read-only and 
    //    /// possible password inside the connection string is hidden (replaced with stars).
    //    /// </remarks>
    //    /// <value>
    //    /// 	<c>true</c> if connection strings should be protected; otherwise, <c>false</c>.
    //    /// </value>
    //    [DefaultValue(false)]
    //    public bool ProtectConnectionStrings { get; set; }

    //    ///// <summary>
    //    ///// Gets or sets the object for which the grid displays properties.
    //    ///// </summary>
    //    ///// <value></value>
    //    ///// <returns>The first object in the object list. If there is no currently selected object the return is null.</returns>
    //    //public new object SelectedObject
    //    //{
    //    //    get { return selectedObject; }
    //    //    set { SelectObject(value); }
    //    //}

    //    ///// <summary>
    //    ///// Gets or sets the currently selected objects.
    //    ///// </summary>
    //    ///// <value></value>
    //    ///// <returns>An array of type <see cref="T:System.Object"/>. The default is an empty array.</returns>
    //    ///// <exception cref="T:System.ArgumentException">One of the items in the array of objects had a null value. </exception>
    //    //public new object[] SelectedObjects
    //    //{
    //    //    get { return selectedObjects; }
    //    //    set { SelectObjects(value); }
    //    //}

    //    protected ToolStrip ToolStrip
    //    {
    //        get
    //        {
    //            if (thisToolStrip == null)
    //            {
    //                var field = typeof(PropertyGrid).GetField("toolStrip",
    //                    BindingFlags.Instance | BindingFlags.NonPublic);
    //                thisToolStrip = (ToolStrip)field.GetValue(this);
    //            }

    //            return thisToolStrip;
    //        }
    //    }

    //    protected bool IsProcessingSelection
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>
    //    /// Initializes this instance.
    //    /// </summary>
    //    private void Initialize()
    //    {
    //        if (alreadyInitialized) return;
    //        if (ToolStrip == null) return;

    //        InitializeComponentModel();
    //        InitializeToolStrip();

    //        alreadyInitialized = true;
    //    }

    //    /// <summary>
    //    /// Initializes the tool strip.
    //    /// </summary>
    //    protected virtual void InitializeToolStrip()
    //    {
    //        ToolStrip.Items.Add(new ToolStripSeparator());
    //        ToolStrip.Items.AddRange(CreateToolStripItems());
    //    }

    //    /// <summary>
    //    /// Creates the tool strip items.
    //    /// </summary>
    //    /// <returns></returns>
    //    protected virtual ToolStripItem[] CreateToolStripItems()
    //    {
    //        return new ToolStripItem[] { };
    //    }

    //    /// <summary>
    //    /// Called when the current selection has changed.
    //    /// </summary>
    //    private void OnSelectionChanged()
    //    {
    //        if (IsProcessingSelection) return;
    //        IsProcessingSelection = true;
    //        try
    //        {
    //            var selection = base.SelectedObjects;
    //            wrappedObjects = ProcessSelection(selection).ToArray();
    //            OnSelectionAboutToChange();
    //            base.SelectedObjects = wrappedObjects;
    //        }
    //        finally { IsProcessingSelection = false; }
    //    }

    //    /// <summary>
    //    /// Processes the current property grid <see cref="SelectedObjects"/>, giving a chance to alter it.
    //    /// </summary>
    //    /// <param name="selection">The selection.</param>
    //    /// <returns>The altered selection</returns>
    //    protected virtual IEnumerable<object> ProcessSelection(IEnumerable<object> selection)
    //    {
    //        if (selection == null) return selection;
    //        return selection
    //            .Select(item => item.GetInnerObject())
    //            .Where(inner => inner != null)
    //            .Select(inner => (object)CreateObjectWrapper(inner));
    //    }

    //    /// <summary>
    //    /// Creates an object wrapper around the original object.
    //    /// </summary>
    //    /// <param name="o">The original object.</param>
    //    /// <returns>Wrapped object.</returns>
    //    protected virtual ObjectWrapper CreateObjectWrapper(object o)
    //    {
    //        return new ObjectWrapper(this, o);
    //    }

    //    /// <summary>
    //    /// Creates the property descriptor used to describe the wrapped object.
    //    /// </summary>
    //    /// <param name="toWrap">The wrapped object.</param>
    //    /// <param name="originalDescriptor">The original property descriptor.</param>
    //    /// <returns>A Property descriptor.</returns>
    //    protected internal virtual PropertyDescriptor CreatePropertyDescriptor(object toWrap, PropertyDescriptor originalDescriptor)
    //    {
    //        return new InnerPropertyDescriptor(toWrap, originalDescriptor, ProtectConnectionStrings);
    //    }

    //    ///// <summary>
    //    ///// Selects the specified object.
    //    ///// </summary>
    //    ///// <param name="o">The object to select.</param>
    //    //protected void SelectObject(object o)
    //    //{
    //    //    if (o == null)
    //    //    {
    //    //        selectedObject = base.SelectedObject = null;
    //    //        return;
    //    //    }

    //    //    selectedObject = o;

    //    //    wrappedObjects = new object[1];
    //    //    wrappedObjects[0] = CreateObjectWrapper(o);
    //    //    OnSelectionAboutToChange();
    //    //    base.SelectedObject = wrappedObjects[0];
    //    //}

    //    ///// <summary>
    //    ///// Selects the specified objects.
    //    ///// </summary>
    //    ///// <param name="objects">The objects to select.</param>
    //    //protected void SelectObjects(object[] objects)
    //    //{
    //    //    if (objects == null)
    //    //    {
    //    //        selectedObjects = base.SelectedObjects = null;
    //    //        return;
    //    //    }

    //    //    selectedObjects = objects;
    //    //    wrappedObjects = new object[objects.Length];

    //    //    for (int i = 0; i < objects.Length; i++)
    //    //        wrappedObjects[i] = CreateObjectWrapper(objects[i]);

    //    //    OnSelectionAboutToChange();
    //    //    base.SelectedObjects = wrappedObjects;
    //    //}
    //}
}
