using System;
using System.Globalization;
using System.Windows.Data;

namespace MapLoading.Converters
{
  public class StringToNewLineConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var wValue = string.Empty;

      if (value != null)
      {
        wValue = value.ToString();

        if (wValue.Contains("\\r\\n"))
          wValue = wValue.Replace("\\r\\n", Environment.NewLine);

        if (wValue.Contains("\\n"))
          wValue = wValue.Replace("\\n", Environment.NewLine);

        if (wValue.Contains("&#x0a;&#x0d;"))
          wValue = wValue.Replace("&#x0a;&#x0d;", Environment.NewLine);

        if (wValue.Contains("&#x0a;"))
          wValue = wValue.Replace("&#x0a;", Environment.NewLine);

        if (wValue.Contains("&#x0d;"))
          wValue = wValue.Replace("&#x0d;", Environment.NewLine);

        if (wValue.Contains("&#10;&#13;"))
          wValue = wValue.Replace("&#10;&#13;", Environment.NewLine);

        if (wValue.Contains("&#10;"))
          wValue = wValue.Replace("&#10;", Environment.NewLine);

        if (wValue.Contains("&#13;"))
          wValue = wValue.Replace("&#13;", Environment.NewLine);

        if (wValue.Contains("<br />"))
          wValue = wValue.Replace("<br />", Environment.NewLine);

        if (wValue.Contains("<LineBreak />"))
          wValue = wValue.Replace("<LineBreak />", Environment.NewLine);
      }

      return wValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}