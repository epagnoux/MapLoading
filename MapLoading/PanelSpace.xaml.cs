using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  public partial class PanelSpace : UserControl
  {
    public enum PanelLayout
    {
      Default,
      Empty,
    }

    public static readonly DependencyProperty LayoutProperty =
      DependencyProperty.Register("Layout",
        typeof (PanelLayout),
        typeof (PanelSpace));


    public PanelSpace()
    {
      InitializeComponent();
    }

    public PanelLayout Layout
    {
      get { return (PanelLayout) GetValue(LayoutProperty); }
      set { SetValue(LayoutProperty, value); }
    }
  }
}