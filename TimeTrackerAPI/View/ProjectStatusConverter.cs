using System;
using System.Globalization;
using System.Windows.Data;

namespace Ficksworkshop.TimeTrackerAPI.View
{
    [ValueConversion(typeof(ProjectStatus), typeof(String))]
    public class ProjectStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is ProjectStatus)
            {
                switch ((ProjectStatus)value)
                {
                    case ProjectStatus.Open:
                        return "Open";
                    case ProjectStatus.Closed:
                        return "Closed";
                }
            }

            throw new Exception("Can only convert ProjectStatus types but the type is " + value.GetType().Name);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Currently, only for display");
        }
    }
}
