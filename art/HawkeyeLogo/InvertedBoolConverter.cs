using System;
using System.Globalization;
using System.Windows.Data;

namespace HawkeyeLogo
{
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof(bool) && targetType != typeof(bool?))
                throw new InvalidOperationException("The target must be a boolean");

            if (value is bool?)
            {
                var bb = (bool?)value;
                if (!bb.HasValue)
                    return bb;

                return new bool?(!bb.Value);
            }

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
