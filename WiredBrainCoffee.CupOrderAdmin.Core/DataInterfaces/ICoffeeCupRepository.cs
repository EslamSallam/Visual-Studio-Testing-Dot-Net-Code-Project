using System.Collections.Generic;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces.Base;

namespace WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces
{
  public interface ICoffeeCupRepository : IRepository<CoffeeCup>
  {
    Task<int> GetCoffeeCupsInStockCountAsync();

    Task<IEnumerable<CoffeeCup>> GetCoffeeCupsInStockAsync(
      int maxItemsToReturn = int.MaxValue);

    Task<IEnumerable<CoffeeCup>> GetAllByOrderIdAsync(int orderId);
  }
}
