using System;
using System.Globalization;
using System.Windows.Data;

namespace Ficksworkshop.TimeTrackerAPI.View
{
    [ValueConversion(typeof(TimeSpan), typeof(String))]
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is TimeSpan)
            {
                return value.ToString();
            }

            throw new Exception("Can only convert ProjectStatus types but the type is " + value.GetType().Name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Currently, only for display");
        }
    }
}
