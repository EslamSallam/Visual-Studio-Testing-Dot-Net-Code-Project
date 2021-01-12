using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WiredBrainCoffee.CupOrderAdmin.UI.Converter
{
  public class OrderCreationSuccessBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var orderCreationSucceeded = (bool)value;

      return orderCreationSucceeded ? Brushes.Green : Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
