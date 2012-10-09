using System;

using Hawkeye.Logging;

namespace Hawkeye.Configuration
{
    partial class SettingsManager
    {
        private class SettingsStore : ISettingsStore
        {
            private static readonly ILogService log = LogManager.GetLogger<SettingsStore>();

            #region ISettingsStore Members

            /// <summary>
            /// Gets or sets the store content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public string Content // TODO: get/set settings store raw content
            {
                get;
                set;
            }

            /// <summary>
            /// Gets a value indicating whether this store is read only.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this store is read only; otherwise, <c>false</c>.
            /// </value>
            public bool IsReadOnly { get { return false; } }

            #endregion
        }

        internal class ReadOnlyStoreWrapper : ISettingsStore
        {
            private ISettingsStore wrapped = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReadOnlyStoreWrapper" /> class.
            /// </summary>
            /// <param name="store">The wrapped settings store.</param>
            /// <exception cref="System.ArgumentNullException"></exception>
            public ReadOnlyStoreWrapper(ISettingsStore store)
            {
                if (store == null) throw new ArgumentNullException("store");
                wrapped = store;
            }

            #region ISettingsStore Members

            /// <summary>
            /// Gets or sets the store content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            /// <exception cref="System.NotSupportedException"></exception>
            public string Content
            {
                get { return wrapped.Content; }
                set
                {
                    throw new NotSupportedException("This Settings Store is Read-Only.");
                }
            }

            /// <summary>
            /// Gets a value indicating whether this store is read only.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this store is read only; otherwise, <c>false</c>.
            /// </value>
            public bool IsReadOnly { get { return true; } }

            #endregion
        }
    }
}
