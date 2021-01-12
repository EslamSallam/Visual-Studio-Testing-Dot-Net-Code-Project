using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Enums;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;
using WiredBrainCoffee.CupOrderAdmin.UI.Command;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel.Base;

namespace WiredBrainCoffee.CupOrderAdmin.UI.ViewModel
{
  public class CustomerViewModel : TabViewModelBase
  {
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICoffeeCupRepository _coffeeCupRepository;
    private readonly IOrderCreationService _orderCreationService;
    private Customer _selectedCustomer;
    private OrderWrapper _selectedOrderWrapper;
    private string _numberOfCupsToOrder;
    private string _orderCreationError;
    private bool _orderCreationSucceeded;

    public CustomerViewModel(ICustomerRepository customerRepository,
        IOrderRepository orderRepository,
        ICoffeeCupRepository coffeeCupRepository,
        IOrderCreationService orderCreationService)
    {
      _customerRepository = customerRepository;
      _orderRepository = orderRepository;
      _coffeeCupRepository = coffeeCupRepository;
      _orderCreationService = orderCreationService;
      Customers = new ObservableCollection<Customer>();
      CustomerOrderWrappers = new ObservableCollection<OrderWrapper>();
      OrderCoffeeCups = new ObservableCollection<CoffeeCup>();
      CreateOrderCommand = new DelegateCommand(CreateOrderExecute, CreateOrderCanExecute);
      SetOrderStatusToProcessedCommand = new DelegateCommand(SetOrderStatusToProcessedExecute, SetOrderStatusToProcessedCanExecute);
    }

    public override async Task LoadAsync()
    {
      if (Customers.Count == 0)
      {
        var customers = await _customerRepository.GetAllAsync();
        foreach (var customer in customers.OrderBy(x => x.FirstName))
        {
          Customers.Add(customer);
        }
      }
    }

    public override string Title => "Customers";

    public ObservableCollection<Customer> Customers { get; }

    public Customer SelectedCustomer
    {
      get => _selectedCustomer;
      set
      {
        if (value == null) return;
        _selectedCustomer = value;
        OnPropertyChanged();
        OnPropertyChanged(nameof(IsCustomerSelected));
        OrderCreationMessage = string.Empty;
        CreateOrderCommand.RaiseCanExecuteChanged();
        LoadCustomerOrdersAsync().Wait();
      }
    }

    public bool IsCustomerSelected => SelectedCustomer != null;

    public ObservableCollection<OrderWrapper> CustomerOrderWrappers { get; }

    public OrderWrapper SelectedOrderWrapper
    {
      get => _selectedOrderWrapper;
      set
      {
        _selectedOrderWrapper = value;
        OnPropertyChanged();
        SetOrderStatusToProcessedCommand.RaiseCanExecuteChanged();
        LoadOrderCoffeeCupsAsync().Wait();
      }
    }

    public ObservableCollection<CoffeeCup> OrderCoffeeCups { get; }

    public DelegateCommand CreateOrderCommand { get; }

    public DelegateCommand SetOrderStatusToProcessedCommand { get; }

    public string NumberOfCupsToOrder
    {
      get { return _numberOfCupsToOrder; }
      set
      {
        _numberOfCupsToOrder = value;
        OnPropertyChanged();
        OrderCreationMessage = string.Empty;
        CreateOrderCommand.RaiseCanExecuteChanged();
      }
    }

    public string OrderCreationMessage
    {
      get => _orderCreationError;
      set
      {
        _orderCreationError = value;
        OnPropertyChanged();
      }
    }

    public bool OrderCreationSucceeded
    {
      get { return _orderCreationSucceeded; }
      set
      {
        _orderCreationSucceeded = value;
        OnPropertyChanged();
      }
    }

    private async Task LoadCustomerOrdersAsync()
    {
      CustomerOrderWrappers.Clear();

      if (SelectedCustomer != null)
      {
        var orders = await _orderRepository.GetAllByCustomerIdAsync(SelectedCustomer.Id);
        foreach (var order in orders.OrderByDescending(x => x.Id))
        {
          CustomerOrderWrappers.Add(new OrderWrapper(order));
        }
      }
    }

    private async Task LoadOrderCoffeeCupsAsync()
    {
      OrderCoffeeCups.Clear();

      if (SelectedOrderWrapper != null)
      {
        var coffeeCups = await _coffeeCupRepository.GetAllByOrderIdAsync(SelectedOrderWrapper.Order.Id);
        foreach (var coffeeCup in coffeeCups.OrderByDescending(x => x.Id))
        {
          OrderCoffeeCups.Add(coffeeCup);
        }
      }
    }

    private async void CreateOrderExecute()
    {
      OrderCreationMessage = "";
      OrderCreationSucceeded = false;

      if (int.TryParse(NumberOfCupsToOrder, out int numberOfCupsToOrder))
      {
        var orderCreationResult = await _orderCreationService.CreateOrderAsync(
          SelectedCustomer, numberOfCupsToOrder);
        if (orderCreationResult.ResultCode == OrderCreationResultCode.Success)
        {
          await LoadCustomerOrdersAsync();
          SelectedOrderWrapper = CustomerOrderWrappers.FirstOrDefault(x => x.Order.Id == orderCreationResult?.CreatedOrder?.Id);
          OrderCreationSucceeded = true;
          OrderCreationMessage = "Created. " +
            $"Remaining cups in stock: {orderCreationResult.RemainingCupsInStock}";
        }
        else if (orderCreationResult.ResultCode == OrderCreationResultCode.StockExceeded)
        {
          OrderCreationMessage = "Stock exceeded. " +
            $"Cups in stock: {orderCreationResult.RemainingCupsInStock}";
        }
        else
        {
          OrderCreationMessage = "Couldn't create order. " +
            $"Result code: {orderCreationResult.ResultCode}";
        }
      }
    }

    private bool CreateOrderCanExecute()
    {
      return IsCustomerSelected
          && int.TryParse(NumberOfCupsToOrder, out int x)
          && x > 0;
    }

    private async void SetOrderStatusToProcessedExecute()
    {
      if (SelectedOrderWrapper != null)
      {
        SelectedOrderWrapper.Status = OrderStatus.Processed;
        await _orderRepository.SaveAsync(SelectedOrderWrapper.Order);

        SetOrderStatusToProcessedCommand.RaiseCanExecuteChanged();
      }
    }

    private bool SetOrderStatusToProcessedCanExecute()
    {
      return SelectedOrderWrapper != null && SelectedOrderWrapper.Status == OrderStatus.Open;
    }
  }

  // Required for change notification of Status property
  // (Status property is bound in DataGrid and changed from
  //  CustomerViewModel's SetOrderStatusToProcessedExecute method)
  public class OrderWrapper : ViewModelBase
  {
    public OrderWrapper(Order order)
    {
      Order = order;
    }

    public Order Order { get; }

    public OrderStatus Status
    {
      get => Order.Status;
      set
      {
        Order.Status = value;
        OnPropertyChanged();
      }
    }
  }
}
