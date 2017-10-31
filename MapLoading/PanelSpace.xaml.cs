using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  public partial class PanelSpace : UserControl
  {
    public enum PanelTemplate
    {
      Default,
      Empty,
    }

    public static readonly DependencyProperty TemplateProperty =
      DependencyProperty.Register("Template",
        typeof (PanelTemplate),
        typeof (PanelSpace));


    public PanelSpace()
    {
      InitializeComponent();
    }

    public PanelTemplate Template
    {
      get { return (PanelTemplate) GetValue(TemplateProperty); }
      set { SetValue(TemplateProperty, value); }
    }
  }
}