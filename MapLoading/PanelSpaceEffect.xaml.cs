using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  /// <summary>
  ///   Interaction logic for PanelSpaceEffect.xaml
  /// </summary>
  public partial class PanelSpaceEffect : UserControl
  {
    public static readonly DependencyProperty CustomContentProperty =
      DependencyProperty.Register("CustomContent",
        typeof (object),
        typeof (PanelSpaceEffect));


    public PanelSpaceEffect()
    {
      InitializeComponent();
    }

    public object CustomContent
    {
      get { return GetValue(CustomContentProperty); }
      set { SetValue(CustomContentProperty, value); }
    }
  }
}