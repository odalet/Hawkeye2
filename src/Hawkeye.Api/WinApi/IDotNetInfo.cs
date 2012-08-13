using System.ComponentModel;

namespace Hawkeye
{
    [TypeConverter(typeof(DotNetInfoConverter))]
    public interface IDotNetInfo
    {
        string Foo { get; }        
    }
}
