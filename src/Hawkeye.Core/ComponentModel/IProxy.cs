namespace Hawkeye.ComponentModel
{
    /// <summary>
    /// Small proxy used to indicate the property editor not to show this object but its proxified value.
    /// </summary>
    internal interface IProxy
    {
        object Value { get; }
    }
}
