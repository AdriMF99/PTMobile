using System.Globalization;

namespace PTMobile.Converters
{
    public class AdminImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAdmin = (bool)value;
            return isAdmin ? "Resources/Images/crownyellow.svg" : "Resources/Images/crown.svg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
