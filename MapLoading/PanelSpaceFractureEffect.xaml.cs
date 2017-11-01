using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  /// <summary>
  ///   Interaction logic for PanelSpaceFractureEffect.xaml
  /// </summary>
  public partial class PanelSpaceFractureEffect : UserControl
  {
    public static readonly DependencyProperty CustomContentProperty =
      DependencyProperty.Register("CustomContent",
        typeof (object),
        typeof (PanelSpaceFractureEffect));


    public PanelSpaceFractureEffect()
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