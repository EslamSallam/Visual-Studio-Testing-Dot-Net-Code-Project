using WiredBrainCoffee.CupOrderAdmin.Core.Model;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation
{
  public class OrderCreationResult
  {
    public OrderCreationResultCode ResultCode { get; set; }

    public Order CreatedOrder { get; set; }

    public int RemainingCupsInStock { get; set; }
  }

  public enum OrderCreationResultCode
  {
    Success,
    StockExceeded
  }
}
