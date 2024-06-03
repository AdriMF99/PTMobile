using System.Globalization;

namespace PTMobile.Converters
{
    public class GodImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAdmin = (bool)value;
            return isAdmin ? "Resources/Images/crownsuperadminyellow.svg" : "Resources/Images/crownsuperadmin.svg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
