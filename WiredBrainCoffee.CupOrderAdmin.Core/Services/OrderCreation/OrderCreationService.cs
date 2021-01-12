using System;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Enums;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation
{
  public class OrderCreationService : IOrderCreationService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly ICoffeeCupRepository _coffeeCupRepository;

    public OrderCreationService(IOrderRepository orderRepository,
      ICoffeeCupRepository coffeeCupRepository)
    {
      _orderRepository =
        orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _coffeeCupRepository =
        coffeeCupRepository ?? throw new ArgumentNullException(nameof(coffeeCupRepository));
    }

    public async Task<OrderCreationResult> CreateOrderAsync(Customer customer,
      int numberOfOrderedCups)
    {
      // TODO: Throw ArgumentOutOfRangeException if number of ordered cups is less than 1

      OrderCreationResult result;

      // TODO: Return StockExceeded result code if not enough cups in stock

      Order createdOrder = await CreateOrderInternalAsync(customer, numberOfOrderedCups);

      result = new OrderCreationResult
      {
        ResultCode = OrderCreationResultCode.Success,
        CreatedOrder = null, // TODO: Store created order in CreatedOrder property
        RemainingCupsInStock = 0 // TODO: Store remaining cups in stock in OrderCreationResult
      };

      return result;
    }

    private static double CalculateDiscountPercentage(CustomerMembership membership,
      int numberOfOrderedCups)
    {
      var discountInPercent = 0.0;

      // TODO: Calculate discount in percent
      //
      //       Rules:
      //       3% for more than 4 cups
      //      
      //       Premium customers get 5% in addition

      return discountInPercent;
    }

    private async Task<Order> CreateOrderInternalAsync(Customer customer,
      int numberOfOrderedCups)
    {
      var discount = CalculateDiscountPercentage(customer.Membership, numberOfOrderedCups);

      var savedOrder = await _orderRepository.SaveAsync(
        new Order
        {
          CustomerId = customer.Id,
          DiscountInPercent = discount,
          Status = OrderStatus.Open
        });

      await LinkCoffeeCupsAsync(savedOrder.Id, numberOfOrderedCups);

      return savedOrder;
    }

    private async Task LinkCoffeeCupsAsync(int orderId, int numberOfOrderedCups)
    {
      var coffeeCupsForOrder =
        await _coffeeCupRepository.GetCoffeeCupsInStockAsync(numberOfOrderedCups);

      foreach (var coffeeCup in coffeeCupsForOrder)
      {
        coffeeCup.OrderId = orderId;
        await _coffeeCupRepository.SaveAsync(coffeeCup);
      }
    }
  }
}
