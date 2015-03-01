using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LoginSocialDemo.Converters
{
    public class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = (bool) value;
            var param = (string) parameter;
            if (result == true && param == Constants.FacebookProvider)
            {
                return Application.Current.Resources["FacebookBlueBackgroundBrush"];
            }
            else if (result == true && param == Constants.GoogleProvider)
            {
                return Application.Current.Resources["GoogleRedBackgroundBrush"];                
            }
            else if (result == true && param == Constants.MicrosoftProvider)
            {
                return Application.Current.Resources["MicrosoftBlueBackgroundBrush"];                
            }
            else
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
