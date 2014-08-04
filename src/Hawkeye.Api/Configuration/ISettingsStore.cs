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
        string Content { get; set; }

        /// <summary>
        /// Gets a value indicating whether this store is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this store is read only; otherwise, <c>false</c>.
        /// </value>
        bool IsReadOnly { get; }
    }
}
