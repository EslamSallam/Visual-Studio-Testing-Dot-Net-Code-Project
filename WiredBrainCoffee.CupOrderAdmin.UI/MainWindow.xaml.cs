using System.Windows;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel;

namespace WiredBrainCoffee.CupOrderAdmin.UI
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      ViewModel = App.GetService<MainViewModel>();
      DataContext = ViewModel;
      Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      await ViewModel.LoadAsync();
    }

    public MainViewModel ViewModel { get; }
  }
}
