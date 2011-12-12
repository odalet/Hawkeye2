using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Hawkeye;
using Hawkeye.Reflection;

namespace ACorns.PropertyGridExtender
{
    /// <summary>
    /// Dynamic Subclass extender for any Property Grid.
    /// This extender will add a small search box to any PropertyGrid to which it is attached.
    /// </summary>
    public class SearchBoxPlugin : IPlugin, IDynamicSubclass
    {
        private PropertyGrid propertyGrid = null;

        private Control toolBar = null;
        private TextBox textBox = null;

        private object gridView = null;

        private IReflectionApi api = null;

        private IFieldAccessor allGridItemsAC;
        private IFieldAccessor topLevelGridItemsAC;

        private IFieldAccessor gridViewEntries;

        private IFieldAccessor totalProps;
        private IFieldAccessor selectedRow;

        private IMethodAccessor setScrollOffset;

        private IPropertyAccessor selectedGridEntry;

        private IMethodAccessor layoutWindow;

        private GridItem[] allGridItems;

        private Color originalBackColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxPlugin"/> class.
        /// </summary>
        public SearchBoxPlugin() { }
        
        #region IPlugin Members

        public void Initialize(IHawkeyeApi hawkeyeApi)
        {
            api = hawkeyeApi.ReflectionApi;
        }

        #endregion

        #region IDynamicSubclass Members

        public void Attach(object target)
        {
            Type targetType = target.GetType();
            if (targetType.FullName == "System.Windows.Forms.PropertyGridInternal.PropertyGridView")
            {
                var ownerGrid = api.CreateFieldAccessor(target, "ownerGrid");
                propertyGrid = ownerGrid.Get() as PropertyGrid;
            }
            else propertyGrid = target as PropertyGrid;

            if (propertyGrid.IsHandleCreated)
                GrabObjects();
            else
            {
                propertyGrid.HandleCreated += (s, e) => GrabObjects();
                propertyGrid.VisibleChanged += (s, e) => GrabObjects();
            }
        }

        #endregion

        private void GrabObjects()
        {
            if (toolBar != null)
                return;

            var toolBarAccesor = api.CreateFieldAccessor(propertyGrid, "toolBar");
            if (toolBarAccesor.IsValid)
                toolBar = toolBarAccesor.Get() as Control;
            else
            {
                toolBarAccesor = api.CreateFieldAccessor(propertyGrid, "toolStrip");
                toolBar = toolBarAccesor.Get() as Control;
                var readonlyFlag = api.CreateFieldAccessor(toolBar.Controls, "_isReadOnly");
                readonlyFlag.Set(false);
            }

            AttachNewToolButtons();

            var gridViewAccesor = api.CreateFieldAccessor(propertyGrid, "gridView");
            gridView = gridViewAccesor.Get();

            allGridItemsAC = api.CreateFieldAccessor( gridView, "allGridEntries");
            topLevelGridItemsAC = api.CreateFieldAccessor(gridView, "topLevelGridEntries");
            totalProps = api.CreateFieldAccessor(gridView, "totalProps");
            selectedRow = api.CreateFieldAccessor(gridView, "selectedRow");

            setScrollOffset = api.CreateMethodAccessor(gridView, "SetScrollOffset");
            selectedGridEntry = api.CreatePropertyAccessor(gridView, "SelectedGridEntry");
            layoutWindow = api.CreateMethodAccessor(gridView, "Refresh");

            propertyGrid.SelectedObjectsChanged += (s, e) => textBox.Text = String.Empty;
            propertyGrid.PropertyTabChanged += (s, e) => textBox.Text = String.Empty;
            propertyGrid.PropertySortChanged += (s, e) => textBox.Text = String.Empty;
        }

        private void AttachNewToolButtons()
        {
            textBox = new TextBox();
            textBox.Location = new Point(0, 0);
            textBox.Size = new Size(70, textBox.Height);
            textBox.BorderStyle = BorderStyle.Fixed3D;
            textBox.Font = new Font("Tahoma", 8.25f);

            originalBackColor = textBox.BackColor;

            textBox.TextChanged += (s, e) => OnTextChanged(((TextBox)s).Text);

            toolBar.Controls.Add(textBox);
            toolBar.SizeChanged += (s, e) => FixSearchBoxLocation();

            FixSearchBoxLocation();
        }

        private void FixSearchBoxLocation()
        {
            textBox.Location = new Point(toolBar.Width - textBox.Width - 2, (toolBar.Height - textBox.Height) / 2);
        }

        private void OnTextChanged(string newText)
        {
            string search = newText.ToLower();

            if (search.Length == 0)
                textBox.BackColor = originalBackColor;
            else
            {
                if (search.Length >= 1 && search.StartsWith("?"))
                    textBox.BackColor = Color.LightBlue;
                else textBox.BackColor = Color.Coral;
            }

            // dynamic filter this grid			
            if (propertyGrid.SelectedObject == null) return;

            var items = GridViewItems;
            if (items == null) return;

            if (search.Length == 0 && allGridItems != null)
            {
                allGridItems = null;
                propertyGrid.Refresh();
                return;
            }

            if (allGridItems == null) allGridItems = items;

            bool checkContains = false;
            if (search.StartsWith("?"))
            {
                search = search.Substring(1);
                checkContains = true;
            }

            var newList = new List<GridItem>();
            foreach (var item in allGridItems)
            {
                if (item.Label == null || item.GridItemType != GridItemType.Property)
                    continue;

                if (checkContains)
                {
                    if (item.Label.ToLower().IndexOf(search) != -1)
                        newList.Add(item);
                }
                else
                {
                    if (item.Label.ToLower().StartsWith(search))
                        newList.Add(item);
                }
            }

            GridViewItems = newList.ToArray();
        }

        private GridItem[] GridViewItems
        {
            get
            {
                var items = allGridItemsAC.Get();
                if (items == null) return null;

                if (gridViewEntries == null)
                    gridViewEntries = api.CreateFieldAccessor(items, "entries");

                gridViewEntries.Target = allGridItemsAC.Get();                
                return gridViewEntries.Get() as GridItem[];
            }
            set
            {
                var items = allGridItemsAC.Get();
                if (items == null) return;

                bool wasFocused = textBox.Focused;

                if (gridViewEntries == null)
                    gridViewEntries = api.CreateFieldAccessor(items, "entries");

                setScrollOffset.Invoke(0);

                gridViewEntries.Target = allGridItemsAC.Get();
                gridViewEntries.Set(value);

                gridViewEntries.Target = topLevelGridItemsAC.Get();
                gridViewEntries.Set(value);

                totalProps.Set(value.Length);
                selectedRow.Set(0);

                if (value.Length > 0) selectedGridEntry.Set(value[0]);

                ((Control)gridView).Invalidate();

                if (wasFocused) textBox.Focus();
            }
        }
    }
}
