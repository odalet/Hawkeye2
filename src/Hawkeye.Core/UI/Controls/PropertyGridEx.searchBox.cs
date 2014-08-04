using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Hawkeye.Reflection;

namespace Hawkeye.UI.Controls
{
    // Search Box implementation
    partial class PropertyGridEx
    {
        private TextBox searchBox = null;
        private Color searchBoxBackColor = Color.White;

        private object reflectedGridView = null;
        private GridItem[] currentGridItems = null;

        private FieldAccessor allGridEntriesAccessor = null;
        private FieldAccessor topLevelGridEntriesAccessor = null;
        private FieldAccessor totalPropsAccessor = null;
        private FieldAccessor selectedRowAccessor = null;
        private FieldAccessor gridViewEntriesAccessor = null;

        private MethodAccessor setScrollOffsetAccessor = null;
        private MethodAccessor refreshAccessor = null;

        private PropertyAccessor selectedGridEntryAccessor = null;

        //private FieldAccessor gridViewAccesor = null;
        //private FieldInfo gridViewField = null;

        private void InitializeSearchBox()
        {
            if (searchBox != null) return;

            //const BindingFlags bflags = BindingFlags.Instance | BindingFlags.NonPublic;

            searchBox = new TextBox();
            searchBox.Location = new Point(0, 0);
            searchBox.Size = new Size(70, searchBox.Height);
            searchBox.BorderStyle = BorderStyle.Fixed3D;
            searchBox.Font = new Font("Tahoma", 8.25f);

            searchBoxBackColor = searchBox.BackColor;

            searchBox.TextChanged += (s, _) => ApplyFilter();
            
            // Hack: let's remove the read-only flag on the toolstrip controls collection
            var rofield = new FieldAccessor(ToolStrip.Controls, "_isReadOnly");
            rofield.Set(false);
            ToolStrip.Controls.Add(searchBox);
            rofield.Set(true);

            ToolStrip.SizeChanged += (s, _) => FixSearchBoxLocation();

            FixSearchBoxLocation();

            // And now initialize accessors
            InitializeAccessors();

            PropertyTabChanged += (s, _) => searchBox.Text = string.Empty;
            PropertySortChanged += (s, _) => searchBox.Text = string.Empty;
            SelectedObjectsChanged += (s, _) => searchBox.Text = string.Empty;
        }

        private void InitializeAccessors()
        {
            var gridViewAccessor = new FieldAccessor(this, "gridView");
            reflectedGridView = gridViewAccessor.Get();
            var gridViewType = reflectedGridView.GetType();

            allGridEntriesAccessor = new FieldAccessor(reflectedGridView, "allGridEntries");
            topLevelGridEntriesAccessor = new FieldAccessor(reflectedGridView, "topLevelGridEntries");
            totalPropsAccessor = new FieldAccessor(reflectedGridView, "totalProps");
            selectedRowAccessor = new FieldAccessor(reflectedGridView, "selectedRow");

            setScrollOffsetAccessor = new MethodAccessor(gridViewType, "SetScrollOffset");
            refreshAccessor = new MethodAccessor(gridViewType, "Refresh");

            selectedGridEntryAccessor = new PropertyAccessor(reflectedGridView, "SelectedGridEntry");
        }

        private void ApplyFilter()
        {
            var search = searchBox.Text.ToLowerInvariant();
            if (string.IsNullOrEmpty(search))
                searchBox.BackColor = searchBoxBackColor;
            else
            {
                if (search.StartsWith("?"))
                    searchBox.BackColor = Color.LightBlue;
                else
                    searchBox.BackColor = Color.Coral;
            }

            if (base.SelectedObject == null) return;

            var items = GetGridViewItems();
            if (items == null)
                return;

            if (string.IsNullOrEmpty(search) && currentGridItems != null)
            {
                currentGridItems = null;
                base.Refresh();
                return;
            }

            if (currentGridItems == null)
                currentGridItems = items;

            var containsMode = search.StartsWith("?");
            if (containsMode)
                search = search.Substring(1);

            // Filter out
            var keptItems = currentGridItems.Where(item =>
            {
                if (string.IsNullOrEmpty(item.Label)) return false;
                if (item.GridItemType != GridItemType.Property) return false;
                var label = item.Label.ToLowerInvariant();

                return containsMode ?
                    label.Contains(search) :
                    label.StartsWith(search);
            }).ToArray();

            SetGridViewItems(keptItems);
        }

        private GridItem[] GetGridViewItems()
        {
            var items = allGridEntriesAccessor.Get();
            if (items == null)
                return null;

            if (gridViewEntriesAccessor == null)
                gridViewEntriesAccessor = new FieldAccessor(items, "entries");

            return gridViewEntriesAccessor.Get(items) as GridItem[];
        }

        private void SetGridViewItems(GridItem[] newItems)
        {
            var items = allGridEntriesAccessor.Get();
            if (items == null)
                return;

            if (newItems == null) newItems = new GridItem[0];

            var wasFocused = searchBox.Focused;

            if (gridViewEntriesAccessor == null)
                gridViewEntriesAccessor = new FieldAccessor(items, "entries");

            setScrollOffsetAccessor.Invoke(reflectedGridView, 0);

            gridViewEntriesAccessor.Set(newItems, items);
            gridViewEntriesAccessor.Set(newItems, topLevelGridEntriesAccessor.Get());

            totalPropsAccessor.Set(newItems.Length);
            selectedRowAccessor.Set(0);

            if (newItems.Length > 0)
                selectedGridEntryAccessor.Set(newItems[0]);

            ((Control)reflectedGridView).Invalidate();

            if (wasFocused)
                searchBox.Focus();
        }

        private void FixSearchBoxLocation()
        {
            searchBox.Location = new Point(
                ToolStrip.Width - searchBox.Width - 2,
                (ToolStrip.Height - searchBox.Height) / 2);
        }
    }
}
