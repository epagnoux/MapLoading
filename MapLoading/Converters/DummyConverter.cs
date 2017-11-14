// =====================================================================================================================
//  Avis de propri�t�:
//  L'information ci-incluse est confidentielle et/ou appartient � CAE Inc. Elle ne peut �tre reproduite ou divulgu�e,
//  en tout ou en partie, et ne peut �tre utilis�e que selon les termes du contrat de licence conclu avec CAE Inc.
//  Copyright CAE Inc., 2017. Tous droits r�serv�s.
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
  public class DummyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}