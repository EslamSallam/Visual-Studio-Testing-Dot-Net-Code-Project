using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel.Base;

namespace WiredBrainCoffee.CupOrderAdmin.UI.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private TabViewModelBase _selectedTabViewModel;

    public MainViewModel(IEnumerable<TabViewModelBase> tabViewModels)
    {
      TabViewModels = new ObservableCollection<TabViewModelBase>(tabViewModels);
      SelectedTabViewModel = TabViewModels.FirstOrDefault();
    }

    public async Task LoadAsync()
    {
      foreach (var tabViewModel in TabViewModels)
      {
        await tabViewModel.LoadAsync();
      }
    }

    public ObservableCollection<TabViewModelBase> TabViewModels { get; }

    public TabViewModelBase SelectedTabViewModel
    {
      get => _selectedTabViewModel;
      set
      {
        if (_selectedTabViewModel != value)
        {
          _selectedTabViewModel = value;
          OnPropertyChanged();

          if (_selectedTabViewModel != null)
          {
            _selectedTabViewModel.LoadAsync().Wait();
          }
        }
      }
    }
  }
}
