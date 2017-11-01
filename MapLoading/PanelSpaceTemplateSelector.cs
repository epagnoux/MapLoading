using System;
using System.Windows;
using System.Windows.Controls;

namespace MapLoading
{
  public class PanelSpaceTemplateSelector : DataTemplateSelector
  {
    public DataTemplate EmptyTemplate { get; set; }
    public DataTemplate DefaultTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (item != null)
        switch ((PanelSpace.PanelLayout)item)
        {
          case PanelSpace.PanelLayout.Empty:
            return EmptyTemplate;

          case PanelSpace.PanelLayout.Default:
            return DefaultTemplate;
          
          default:
            return DefaultTemplate;
        }
      return EmptyTemplate;
    }
  }
}