using System;
using System.Globalization;
using System.Windows.Data;

namespace MapLoading.Converters
{
  public class DoubleToIncreaseConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value != null?double.Parse(value.ToString()) + double.Parse(parameter.ToString()) : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}