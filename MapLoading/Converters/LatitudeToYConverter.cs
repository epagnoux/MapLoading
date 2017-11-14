// =====================================================================================================================
//  Avis de propriété:
//  L'information ci-incluse est confidentielle et/ou appartient à CAE Inc. Elle ne peut être reproduite ou divulguée,
//  en tout ou en partie, et ne peut être utilisée que selon les termes du contrat de licence conclu avec CAE Inc.
//  Copyright CAE Inc., 2017. Tous droits réservés.
// 
//  Proprietary notice:
//  The information contained herein is confidential and/or proprietary to CAE Inc. It shall not be reproduced or
//  disclosed in whole or in part, and may only be used in accordance with the terms of the License Agreement entered
//  into with CAE Inc.
//  Copyright CAE Inc., 2017. All Rights Reserved.
// =====================================================================================================================

using System;
using System.Globalization;
using System.Windows.Data;

namespace MapLoading.Converters
{
  public class LatitudeToYConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values[0] != null && values[1] != null)
      {
        var wLatitude = (double)values[0];
        var wHeight = double.Parse(values[1].ToString());
        var wOffset = 0.0;
        if (values[2] != null)
        {
          wOffset = double.Parse(values[2].ToString());
        }
        return (wHeight / 180) * (90 - wLatitude) + wOffset;
      }
      return 0.0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}