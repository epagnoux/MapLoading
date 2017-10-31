using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  /// <summary>
  ///   Interaction logic for PanelSpaceEffect.xaml
  /// </summary>
  public partial class PanelSpaceEffect : UserControl
  {
    public static readonly DependencyProperty ContentProperty =
      DependencyProperty.Register("Content",
        typeof (object),
        typeof (PanelSpaceEffect));


    public PanelSpaceEffect()
    {
      InitializeComponent();
    }

    public object Content
    {
      get { return GetValue(ContentProperty); }
      set { SetValue(ContentProperty, value); }
    }
  }
}