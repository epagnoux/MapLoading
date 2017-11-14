
namespace MapLoading.Controls.MapLoader
{
  /// <summary>
  /// Interaction logic for MapLoader.xaml
  /// </summary>
  public partial class MapLoader //: IViewModel<MapLoaderVM>
  {
    
    public MapLoader()
    {
      ViewModel = new MapLoaderVM();
      DataContext = ViewModel;

      InitializeComponent();
    }
    public MapLoaderVM ViewModel { get; set; }
  }
}
