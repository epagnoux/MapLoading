using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  /// <summary>
  ///   Interaction logic for PanelSpaceEffect.xaml
  /// </summary>
  public partial class PanelSpaceEffect : UserControl
  {
    public static readonly DependencyProperty MyContentProperty =
      DependencyProperty.Register("MyContent",
        typeof (object),
        typeof (PanelSpaceEffect));


    public PanelSpaceEffect()
    {
      InitializeComponent();
    }

    public object MyContent
    {
      get { return GetValue(MyContentProperty); }
      set { SetValue(MyContentProperty, value); }
    }
  }
}