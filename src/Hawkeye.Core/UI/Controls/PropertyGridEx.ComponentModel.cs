//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.Design;

//namespace Hawkeye.UI.Controls
//{
//    partial class PropertyGridEx
//    {
//        /// <summary>
//        /// Partial implementation (only the verbs are available) of
//        /// <see cref="System.ComponentModel.Design.IMenuCommandService"/>.
//        /// </summary>
//        private class MenuCommandService : IMenuCommandService
//        {
//            private DesignerVerbCollection verbsCollection = null;

//            /// <summary>
//            /// Initializes a new instance of the <see cref="MenuCommandService"/> class.
//            /// </summary>
//            public MenuCommandService() : this(null) { }

//            /// <summary>
//            /// Initializes a new instance of the <see cref="MenuCommandService"/> class.
//            /// </summary>
//            /// <param name="verbs">The verbs.</param>
//            public MenuCommandService(DesignerVerbCollection verbs) { verbsCollection = verbs; }

//            #region IMenuCommandService Members

//            #region Commands: not implemented

//            /// <summary>
//            /// Adds the specified standard menu command to the menu.
//            /// </summary>
//            /// <param name="command">The <see cref="T:System.ComponentModel.Design.MenuCommand"/> to add.</param>
//            /// <exception cref="T:System.InvalidOperationException">
//            /// The <see cref="T:System.ComponentModel.Design.CommandID"/> of the specified <see cref="T:System.ComponentModel.Design.MenuCommand"/> is already present on a menu.
//            /// </exception>
//            public void AddCommand(MenuCommand command)
//            {
//                // Intentional
//                throw new NotSupportedException();
//            }

//            /// <summary>
//            /// Searches for the specified command ID and returns the menu command associated with it.
//            /// </summary>
//            /// <param name="commandID">The <see cref="T:System.ComponentModel.Design.CommandID"/> to search for.</param>
//            /// <returns>
//            /// The <see cref="T:System.ComponentModel.Design.MenuCommand"/> associated with the command ID, or null if no command is found.
//            /// </returns>
//            public MenuCommand FindCommand(CommandID commandID)
//            {
//                // Intentional
//                throw new NotSupportedException();
//            }

//            /// <summary>
//            /// Invokes a menu or designer verb command matching the specified command ID.
//            /// </summary>
//            /// <param name="commandID">The <see cref="T:System.ComponentModel.Design.CommandID"/> of the command to search for and execute.</param>
//            /// <returns>
//            /// true if the command was found and invoked successfully; otherwise, false.
//            /// </returns>
//            public bool GlobalInvoke(CommandID commandID)
//            {
//                // Intentional
//                throw new NotSupportedException();
//            }

//            /// <summary>
//            /// Removes the specified standard menu command from the menu.
//            /// </summary>
//            /// <param name="command">The <see cref="T:System.ComponentModel.Design.MenuCommand"/> to remove.</param>
//            public void RemoveCommand(MenuCommand command)
//            {
//                // Intentional
//                throw new NotSupportedException();
//            }

//            /// <summary>
//            /// Shows the specified shortcut menu at the specified location.
//            /// </summary>
//            /// <param name="menuID">The <see cref="T:System.ComponentModel.Design.CommandID"/> for the shortcut menu to show.</param>
//            /// <param name="x">The x-coordinate at which to display the menu, in screen coordinates.</param>
//            /// <param name="y">The y-coordinate at which to display the menu, in screen coordinates.</param>
//            public void ShowContextMenu(CommandID menuID, int x, int y)
//            {
//                // Intentional
//                throw new NotSupportedException();
//            }

//            #endregion

//            #region Verbs

//            /// <summary>
//            /// Gets or sets an array of the designer verbs that are currently available.
//            /// </summary>
//            /// <value></value>
//            /// <returns>
//            /// An array of type <see cref="T:System.ComponentModel.Design.DesignerVerb"/> that indicates the designer verbs that are currently available.
//            /// </returns>
//            public DesignerVerbCollection Verbs
//            {
//                get { return verbsCollection; }
//            }

//            /// <summary>
//            /// Adds the specified designer verb to the set of global designer verbs.
//            /// </summary>
//            /// <param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"/> to add.</param>
//            public void AddVerb(DesignerVerb verb)
//            {
//                if ((verbsCollection != null) && (verb != null)) verbsCollection.Add(verb);
//            }

//            /// <summary>
//            /// Removes the specified designer verb from the collection of global designer verbs.
//            /// </summary>
//            /// <param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"/> to remove.</param>
//            public void RemoveVerb(DesignerVerb verb)
//            {
//                if ((verbsCollection != null) && verbsCollection.Contains(verb))
//                    verbsCollection.Remove(verb);
//            }

//            #endregion

//            #endregion
//        }

//        private class PropertyGridExContainer : Container
//        {
//            public class Site : ISite
//            {
//                private IComponent component = null;
//                private string componentName = string.Empty;
//                private PropertyGridExContainer container = null;
//                private PropertyGridEx grid = null;

//                public Site(IComponent component, string name, PropertyGridExContainer container, PropertyGridEx parentGrid)
//                {
//                    if (parentGrid == null) throw new ArgumentNullException("parentGrid");
//                    if (component == null) throw new ArgumentNullException("component");
//                    if (container == null) throw new ArgumentNullException("container");

//                    this.component = component;
//                    this.container = container;
//                    componentName = name;
//                    grid = parentGrid;
//                }

//                #region ISite Members

//                public IComponent Component { get { return component; } }

//                public IContainer Container { get { return container; } }

//                public bool DesignMode { get { return false; } }

//                public string Name
//                {
//                    get { return componentName; }
//                    set { componentName = value; }
//                }

//                #endregion

//                #region IServiceProvider Members

//                public object GetService(Type serviceType)
//                {
//                    return container.GetService(serviceType);
//                }

//                #endregion
//            }

//            private ServiceContainer services = null;
//            private PropertyGridEx grid = null;

//            public PropertyGridExContainer(PropertyGridEx parentGrid)
//                : base()
//            {
//                if (parentGrid == null) throw new ArgumentNullException("parentGrid");
//                grid = parentGrid;
//                services = new ServiceContainer();
//            }

//            public IServiceContainer Services { get { return services; } }

//            protected override object GetService(Type service)
//            {
//                var serviceInstance = services.GetService(service);
//                if (serviceInstance == null)
//                    return base.GetService(service);
//                else return serviceInstance;
//            }

//            protected override ISite CreateSite(IComponent component, string name)
//            {
//                return new Site(component, name, this, grid);
//            }
//        }

//        private DesignerVerbCollection verbs = new DesignerVerbCollection();
//        private PropertyGridExContainer container = null;

//        #region Public methods to add/remove verbs

//        /// <summary>
//        /// Adds the specified designer verb to the set of global designer verbs.
//        /// </summary>
//        /// <param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"/> to add.</param>
//        public void AddVerb(DesignerVerb verb)
//        {
//            if (verb != null) verbs.Add(verb);
//        }

//        /// <summary>
//        /// Removes the specified designer verb from the collection of global designer verbs.
//        /// </summary>
//        /// <param name="verb">The <see cref="T:System.ComponentModel.Design.DesignerVerb"/> to remove.</param>
//        public void RemoveVerb(DesignerVerb verb)
//        {
//            if (verbs.Contains(verb)) verbs.Remove(verb);
//        }

//        #endregion

//        private void InitializeComponentModel()
//        {
//            container = new PropertyGridExContainer(this);
//            container.Services.AddService<IMenuCommandService>(new MenuCommandService(verbs));
//        }

//        private void OnSelectionAboutToChange()
//        {
//            // We clear the components
//            var components = new List<IComponent>(container.Components.Cast<IComponent>());
//            components.ForEach(c => container.Remove(c));

//            // We add the new selection to the container.
//            foreach (var c in wrappedObjects.Cast<IComponent>())
//            {
//                c.Site = new PropertyGridExContainer.Site(c, null, container, this);
//                container.Add(c);
//            }
//        }
//    }
//}
