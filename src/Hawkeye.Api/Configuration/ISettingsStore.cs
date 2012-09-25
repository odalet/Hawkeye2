using System;
using System.Xml;

namespace Hawkeye.Configuration
{
    /// <summary>
    /// Represents a section of the hawkeye.settings file
    /// </summary>
    public interface ISettingsStore
    {
        /// <summary>
        /// Gets or sets the store content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this store is readonly.
        /// </summary>
        /// <value>
        /// <c>true</c> if this store is readonly; otherwise, <c>false</c>.
        /// </value>
        bool IsReadonly { get; }
    }
}
