using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.UI.Command;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel.Base;

namespace WiredBrainCoffee.CupOrderAdmin.UI.ViewModel
{
  public class CoffeeCupViewModel : TabViewModelBase
  {
    private readonly ICoffeeCupRepository _coffeeCupRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private int _numberOfCupsOrdered;
    private int _numberOfCupsProduced;

    private CoffeeCup _selectedCoffeeCup;
    private Order _selectedOrder;
    private Customer _selectedCustomer;

    public CoffeeCupViewModel(ICoffeeCupRepository coffeeCupRepository,
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository)
    {
      _coffeeCupRepository = coffeeCupRepository;
      _customerRepository = customerRepository;
      _orderRepository = orderRepository;
      CoffeeCups = new ObservableCollection<CoffeeCup>();
      ProduceCoffeeCupCommand = new DelegateCommand(ProduceCoffeeCupExecute);
    }

    public async override Task LoadAsync()
    {
      CoffeeCups.Clear();

      var coffeeCups = await _coffeeCupRepository.GetAllAsync();
      foreach (var coffeeCup in coffeeCups.OrderByDescending(x => x.Id))
      {
        CoffeeCups.Add(coffeeCup);
      }

      NumberOfCupsProduced = CoffeeCups.Count;
      NumberOfCupsOrdered = CoffeeCups.Count(x => x.OrderId.HasValue);
    }

    public override string Title => "Coffee Cups in Stock";

    public int NumberOfCupsProduced
    {
      get => _numberOfCupsProduced;
      set
      {
        _numberOfCupsProduced = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(NumberOfCupsInStock));
      }
    }

    public int NumberOfCupsOrdered
    {
      get => _numberOfCupsOrdered;
      set
      {
        _numberOfCupsOrdered = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(NumberOfCupsInStock));
      }
    }

    public int NumberOfCupsInStock => NumberOfCupsProduced - _numberOfCupsOrdered;

    public ObservableCollection<CoffeeCup> CoffeeCups { get; }

    public CoffeeCup SelectedCoffeeCup
    {
      get { return _selectedCoffeeCup; }
      set
      {
        _selectedCoffeeCup = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(IsSelectedCoffeeCupPartOfOrder));
        LoadOrderAndCustomerAsync().Wait();
      }
    }

    public bool IsSelectedCoffeeCupPartOfOrder => SelectedCoffeeCup?.OrderId != null;

    public Order SelectedOrder

    {
      get => _selectedOrder;
      set
      {
        _selectedOrder = value;
        OnPropertyChanged();
      }
    }

    public Customer SelectedCustomer
    {
      get { return _selectedCustomer; }
      set
      {
        _selectedCustomer = value;
        OnPropertyChanged();
      }
    }

    public ICommand ProduceCoffeeCupCommand { get; }

    public async void ProduceCoffeeCupExecute()
    {
      await _coffeeCupRepository.SaveAsync(new CoffeeCup());
      await this.LoadAsync();
    }

    private async Task LoadOrderAndCustomerAsync()
    {
      if (SelectedCoffeeCup?.OrderId == null)
      {
        SelectedOrder = null;
        SelectedCustomer = null;
      }
      else
      {
        SelectedOrder = await _orderRepository.GetByIdAsync(SelectedCoffeeCup.OrderId.Value);
        SelectedCustomer = await _customerRepository.GetByIdAsync(SelectedOrder.CustomerId);
      }
    }
  }
}
