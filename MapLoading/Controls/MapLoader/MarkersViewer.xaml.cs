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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MapLoading.Controls.MapLoader.Models;

namespace MapLoading.Controls.MapLoader
{
  /// <summary>
  ///   Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class MarkersViewer : UserControl
  {
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(MarkersViewer), new PropertyMetadata(OnItemsChanged));

    public MarkersViewer()
    {
      InitializeComponent();
    }


    public IEnumerable<object> ItemsSource
    {
      get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wSender = (MarkersViewer) d;
    }
  }
}