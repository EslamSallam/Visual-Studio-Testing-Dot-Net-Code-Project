using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories.Base;

namespace WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories
{
  public class CoffeeCupFileRepository : FileRepositoryBase<CoffeeCup>, ICoffeeCupRepository
  {
    public CoffeeCupFileRepository() : base(@"CoffeeCups.json")
    {
    }

    public async Task<IEnumerable<CoffeeCup>> GetCoffeeCupsInStockAsync(
        int maxItemsToReturn = int.MaxValue)
    {
      var allCoffeeCups = await GetAllAsync();
      return allCoffeeCups
          .Where(x => !x.OrderId.HasValue)
          .OrderBy(x => x.Id)
          .Take(maxItemsToReturn)
          .ToList();
    }

    public async Task<int> GetCoffeeCupsInStockCountAsync()
    {
      var availableCoffeeCups = await GetCoffeeCupsInStockAsync();
      return availableCoffeeCups.Count();
    }

    public async Task<IEnumerable<CoffeeCup>> GetAllByOrderIdAsync(int orderId)
    {
      var allItems = await GetAllAsync();
      return allItems.Where(x => x.OrderId == orderId);
    }
  }
}
