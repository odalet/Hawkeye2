using System;
using System.Xml;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;

using Hawkeye.Logging;

namespace Hawkeye.Configuration
{
    internal class LayoutService
    {
        private class FormLayoutData
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FormLayoutData" /> class.
            /// </summary>
            public FormLayoutData() : this(null) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="FormLayoutData" /> class from the data in the specified form.
            /// </summary>
            public FormLayoutData(Form form)
            {
                if (form == null)
                    AdditionalData = string.Empty;
                else
                {
                    if (form is IAdditionalLayoutDataProvider)
                        AdditionalData = ((IAdditionalLayoutDataProvider)form).GetAdditionalLayoutData();
                    Bounds = form.Bounds;
                    WindowState = form.WindowState;
                    ScreenName = Screen.PrimaryScreen.DeviceName;
                }
            }

            public Rectangle? Bounds { get; set; }
            public FormWindowState? WindowState { get; set; }
            public string ScreenName { get; set; }
            public string AdditionalData { get; set; }
        }

        private class EventsSuspender : IDisposable
        {
            private LayoutService service = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventsSuspender" /> class.
            /// </summary>
            /// <param name="layoutManager">The layout manager.</param>
            public EventsSuspender(LayoutService layoutService)
            {
                service = layoutService;
                service.eventsAreSuspended = true;
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                service.eventsAreSuspended = false;
            }

            #endregion
        }

        private static readonly ILogService log = LogManager.GetLogger<LayoutService>();

        private bool eventsAreSuspended = false;
        private Rectangle defaultBounds = Rectangle.Empty;
        private List<string> loadedList = new List<string>();
        private Dictionary<string, Form> forms = new Dictionary<string, Form>();
        private Dictionary<string, FormLayoutData> layouts = new Dictionary<string, FormLayoutData>();

        private Func<XmlNode> getLayoutsRootNode = null;

        public LayoutService(Func<XmlNode> layoutsRootNodeFunc)
        {
            getLayoutsRootNode = layoutsRootNodeFunc;
            LoadLayouts();
        }

        private bool EventsLocked
        {
            get { return eventsAreSuspended; }
        }

        /// <summary>
        /// Suspends the forms events from being observed by this service.
        /// </summary>
        /// <returns>An <see cref="System.IDisposable"/> object that when released, resumes events observation.</returns>
        private IDisposable SuspendEvents()
        {
            return new EventsSuspender(this);
        }

        #region ILayoutService Members

        /// <summary>
        /// Registers a form with the service.
        /// </summary>
        /// <param name="key">The key identifying the form class.</param>
        /// <param name="form">The form instance.</param>
        public void RegisterForm(string key, Form form)
        {
            if (forms.ContainsKey(key)) forms.Remove(key);
            forms.Add(key, form);
            LoadForm(key);
            form.Load += (s, e) => LoadForm(key);
        }

        #endregion

        #region Xml deserialization

        //internal void ReadLayouts(string filename)
        //{
        //    try
        //    {
        //        var doc = new XmlDocument();
        //        doc.Load(filename);
        //        ReadLayouts(doc);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(string.Format(
        //            "Unable to deserialize layout information from file {0}.", filename), ex);
        //    }
        //}

        private void LoadLayouts()
        {
            LoadLayouts(getLayoutsRootNode());
        }

        private void LoadLayouts(XmlNode rootLayoutsNode)
        {
            foreach (var layoutNode in rootLayoutsNode.ChildNodes.Cast<XmlNode>().Where(xn => xn.Name == "layout"))
            {
                var key = GetNodeValue(layoutNode, "key");
                if (key == null) continue;
                try
                {
                    LoadLayout(layoutNode, key);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format(
                        "Unable to deserialize layout information for key {0}.", key), ex);
                }
            }
        }

        private void LoadLayout(XmlNode layoutNode, string key)
        {
            if (layouts.ContainsKey(key)) return;

            var layoutData = new FormLayoutData();
            foreach (XmlNode xn in layoutNode.ChildNodes)
            {
                switch (xn.Name)
                {
                    case "bounds":
                        var bounds = GetNodeValue(xn);
                        if (bounds != null)
                        {
                            try
                            {
                                layoutData.Bounds = ConvertTo<Rectangle>(bounds);
                            }
                            catch (Exception ex)
                            {
                                log.Warning(string.Format(
                                    "Invalid 'bounds' value for key {0}: {1}",
                                    key, bounds), ex);
                                layoutData.Bounds = null;
                            }
                        }
                        else
                        {
                            log.Warning(string.Format(
                                "'bounds' value for key {0} was not found.",
                                key));
                            layoutData.Bounds = null;
                        }
                        break;
                    case "state":
                        var state = GetNodeValue(xn);
                        if (state != null)
                        {
                            try
                            {
                                layoutData.WindowState = ConvertTo<FormWindowState>(state);
                            }
                            catch (Exception ex)
                            {
                                log.Warning(string.Format(
                                    "Invalid 'state' value for key {0}: {2}",
                                    key, state), ex);
                                layoutData.WindowState = null;
                            }
                        }
                        else
                        {
                            log.Warning(string.Format(
                                "'state' value for key {0} was not found.",
                                key));
                            layoutData.WindowState = null;
                        }
                        break;
                    case "screen":
                        layoutData.ScreenName = GetNodeValue(xn);
                        break;
                    case "additionalData":
                        layoutData.AdditionalData = xn.InnerText;
                        break;
                }
            }
            layouts.Add(key, layoutData);
        }

        private string GetNodeValue(XmlNode xn)
        {
            return GetNodeValue(xn, "value");
        }

        private string GetNodeValue(XmlNode xn, string xaName)
        {
            var xaValue = xn.Attributes.Cast<XmlAttribute>().FirstOrDefault(
                        xa => xa.Name == xaName);
            if (xaValue != null)
                return xaValue.Value;
            else return null;
        }

        #endregion

        #region Xml serialization

        internal void SaveLayouts()
        {
            SaveLayouts(getLayoutsRootNode());
        }

        private void SaveLayouts(XmlNode rootLayoutsNode)
        {

            var doc = rootLayoutsNode is XmlDocument ?
                (XmlDocument)rootLayoutsNode : rootLayoutsNode.OwnerDocument;

            Func<string, XmlNode> findLayoutNode = k =>
            {
                var layoutNodes = rootLayoutsNode.ChildNodes.Cast<XmlNode>().Where(n =>
                    n.Name == "layout");
                foreach (var node in layoutNodes)
                {
                    var xaKey = node.Attributes.Cast<XmlAttribute>().SingleOrDefault(a => a.Name == "key");
                    if (xaKey.Value == k) return node;
                }

                return null;
            };

            foreach (string key in layouts.Keys)
            {
                var xn = findLayoutNode(key);
                if (xn == null)
                {
                    xn = doc.CreateElement("layout");
                    var xa = doc.CreateAttribute("key");
                    xa.Value = key;
                    xn.Attributes.Append(xa);
                }

                // mandatory, if not present, skip this node
                var layout = layouts[key];
                if (layout == null) continue;

                if (layout.Bounds.HasValue)
                {
                    var boundsValue = ConvertToString(layout.Bounds);
                    var xnBounds = xn.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name == "bounds");
                    if (xnBounds != null)
                        xnBounds.Attributes["value"].Value = boundsValue;
                    else
                    {
                        xnBounds = xn.AppendChild(doc.CreateElement("bounds"));
                        var xaBoundsValue = doc.CreateAttribute("value");
                        xaBoundsValue.Value = boundsValue;
                        xnBounds.Attributes.Append(xaBoundsValue);
                    }
                }

                if (layout.WindowState.HasValue)
                {
                    var stateValue = ConvertToString(layout.WindowState);
                    var xnState = xn.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name == "state");
                    if (xnState != null)
                        xnState.Attributes["value"].Value = stateValue;
                    else
                    {
                        xnState = xn.AppendChild(doc.CreateElement("state"));
                        var xaStateValue = doc.CreateAttribute("value");
                        xaStateValue.Value = stateValue;
                        xnState.Attributes.Append(xaStateValue);
                    }
                }

                if (!string.IsNullOrEmpty(layout.ScreenName))
                {
                    var screenValue = layout.ScreenName;
                    var xnScreen = xn.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name == "screen");
                    if (xnScreen != null)
                        xnScreen.Attributes["value"].Value = screenValue;
                    else
                    {
                        xnScreen = xn.AppendChild(doc.CreateElement("screen"));
                        var xaScreenValue = doc.CreateAttribute("value");
                        xaScreenValue.Value = screenValue;
                        xnScreen.Attributes.Append(xaScreenValue);
                    }
                }

                if (layout.AdditionalData != null)
                {
                    var xnAdditionalData = xn.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name == "additionalData");
                    if (xnAdditionalData == null)
                        xnAdditionalData = xn.AppendChild(doc.CreateElement("additionalData"));
                    xnAdditionalData.InnerText = layout.AdditionalData;
                }

                rootLayoutsNode.AppendChild(xn);
            }

            // Regular settings have already been saved, but we happen after that. So, save once again
            SettingsManager.Save();  
        }

        #endregion

        #region Registered forms events

        private bool IsLoaded(string key)
        {
            return loadedList.Contains(key);
        }

        /// <summary>
        /// Restores the state and bounds of the form identified by the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void LoadForm(string key)
        {
            if (IsLoaded(key)) return;

            using (SuspendEvents())
            {
                if (!forms.ContainsKey(key)) return;

                var form = forms[key];
                var defaultScreen = Screen.PrimaryScreen;

                if (layouts.ContainsKey(key))
                {
                    var layout = layouts[key];

                    // Determine screen
                    var screenName = layout.ScreenName;
                    var screen = defaultScreen;
                    if (!string.IsNullOrEmpty(screenName)) screen =
                        Screen.AllScreens.SingleOrDefault(s => s.DeviceName == screenName) ?? defaultScreen;

                    if (layout.Bounds.HasValue)
                    {
                        form.StartPosition = FormStartPosition.Manual;
                        var bounds = layout.Bounds.Value;
                        if (!screen.WorkingArea.Contains(bounds))
                            SetDefaultLayout(form);
                        else form.Bounds = bounds;
                    }
                    else log.Verbose(string.Format("Could not find a 'bounds' value for layout key {0}", key));

                    if (layout.WindowState.HasValue)
                    {
                        var state = layout.WindowState.Value;
                        if (state == FormWindowState.Minimized)
                            form.WindowState = FormWindowState.Normal;
                        else form.WindowState = state;
                    }
                    else
                    {
                        // default to Normal
                        form.WindowState = FormWindowState.Normal;
                        log.Verbose(string.Format("Could not find a 'state' value for layout key {0}", key));
                    }

                    // If state is Normal and we have no bounds, use DefaultBounds and center on screen
                    if ((!layout.Bounds.HasValue) && (form.WindowState == FormWindowState.Normal))
                        SetDefaultLayout(form);
                }
                else
                {
                    SetDefaultLayout(form);
                    layouts.Add(key, new FormLayoutData(form));
                }

                form.SizeChanged += (s, e) => UpdateForm(key);
                form.LocationChanged += (s, e) => UpdateForm(key);
                form.FormClosed += (s, e) => UnloadForm(key);

                loadedList.Add(key);

                UpdateForm(key);

                // Even if no additional data was saved, call SetData.
                if (form is IAdditionalLayoutDataProvider)
                    ((IAdditionalLayoutDataProvider)form).SetAdditionalLayoutData(layouts[key].AdditionalData);
            }
        }

        /// <summary>
        /// Updates the bounds and state information of the form identified by the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void UpdateForm(string key)
        {
            if (EventsLocked) return;
            if (!IsLoaded(key)) return;

            using (SuspendEvents())
            {
                if (!forms.ContainsKey(key)) return;
                var form = forms[key];
                var layout = layouts[key];

                layout.WindowState = form.WindowState;
                layout.Bounds = form.Bounds;
                layout.ScreenName = Screen.FromControl(form).DeviceName;
            }
        }

        /// <summary>
        /// Saves the bounds and state information of the form identified by the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void UnloadForm(string key)
        {
            UpdateForm(key);

            if (layouts.ContainsKey(key))
            {
                var layout = layouts[key];
                var form = forms[key];
                if (form is IAdditionalLayoutDataProvider)
                    layout.AdditionalData = ((IAdditionalLayoutDataProvider)form).GetAdditionalLayoutData();
            }

            SaveLayouts();

            loadedList.Remove(key);
        }

        #endregion

        #region Utilities

        private void SetDefaultLayout(Form form)
        {
            form.WindowState = FormWindowState.Normal;
            if (form is IDefaultLayoutProvider)
                form.Bounds = ((IDefaultLayoutProvider)form).GetDefaultBounds();
        }

        private static T ConvertTo<T>(string value)
        {
            return (T)ConvertToType(value, typeof(T));
        }

        /// <summary>
        /// Converts a string into an object of type <paramref name="targetType"/>.
        /// </summary>
        /// <remarks>
        /// This method is used by <see cref="Sopra.Collections.DictionarySerializer"/>.
        /// </remarks>
        /// <param name="value">The string value.</param>
        /// <param name="targetType">Type of the target object.</param>
        /// <returns>An object of the correct type containing the converted value (or null).</returns>
        private static object ConvertToType(string value, Type targetType)
        {
            if (targetType == typeof(string)) return value;
            else
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    // first, we try to convert using invariant culture.
                    try { return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value); }
                    catch (Exception ex)
                    {
                        log.Verbose(string.Format(
                            "Unable to convert {0} to a {1} using invariant culture.",
                            value, targetType), ex);

                        // Then, second chance: we use the current culture.
                        return converter.ConvertFrom(null, Thread.CurrentThread.CurrentCulture, value);
                    }
                }
            }

            return null;
        }

        private static string ConvertToString(object value)
        {
            if (value is string) return (string)value;
            else if (value == null) return string.Empty;
            else
            {
                var objectType = value.GetType();
                var converter = TypeDescriptor.GetConverter(objectType);

                // we always convert using invariant culture.
                if (converter.CanConvertTo(typeof(string)))
                    return converter.ConvertToInvariantString(value);
                else return value.ToString();
            }
        }

        #endregion
    }
}
