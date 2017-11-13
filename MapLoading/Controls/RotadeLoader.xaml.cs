using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls.ColorEditor.ColorSchemas;

namespace MapLoading
{
	/// <summary>
	/// Interaction logic for RotadeLoader.xaml
	/// </summary>
	public partial class RotadeLoader : UserControl
	{
	  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
	    "Color", typeof (Brush), typeof (RotadeLoader), new PropertyMetadata(Brushes.White));

	  public Brush Color
	  {
	    get { return (Brush) GetValue(ColorProperty); }
	    set { SetValue(ColorProperty, value); }
	  }
		public RotadeLoader()
		{
      if (double.IsNaN(Height))
        Height = 100;
      if (double.IsNaN(Width))
		    Width = 100;
		  
			this.InitializeComponent();
      SizeChanged += RotadeLoaderSizeChanged;
		}

    void RotadeLoaderSizeChanged(object sender, SizeChangedEventArgs e)
    {
      Height = Height = Math.Max(e.NewSize.Height, e.NewSize.Width);
      Width = Height = Math.Max(e.NewSize.Height, e.NewSize.Width);
    }
	}
}