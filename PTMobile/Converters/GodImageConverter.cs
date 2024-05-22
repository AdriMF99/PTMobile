using System.Globalization;

namespace PTMobile.Converters
{
    public class GodImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAdmin = (bool)value;
            return isAdmin ? "Resources/Images/zeusconrelleno.png" : "Resources/Images/zeussinrelleno.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
