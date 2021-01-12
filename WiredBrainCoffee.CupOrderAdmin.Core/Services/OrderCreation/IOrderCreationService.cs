using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation
{
  public interface IOrderCreationService
  {
    Task<OrderCreationResult> CreateOrderAsync(Customer customer, int numberOfOrderedCups);
  }
}
