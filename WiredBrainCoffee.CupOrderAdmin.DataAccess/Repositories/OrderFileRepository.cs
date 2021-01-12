using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories.Base;

namespace WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories
{
  public class OrderFileRepository : FileRepositoryBase<Order>, IOrderRepository
  {
    public OrderFileRepository() : base(@"Orders.json")
    {
    }

    public async Task<IEnumerable<Order>> GetAllByCustomerIdAsync(int customerId)
    {
      var allItems = await GetAllAsync();
      return allItems.Where(x => x.CustomerId == customerId);
    }

    public async Task<int> GetCountByCustomerIdAsync(int customerId)
    {
      var allItems = await GetAllAsync();
      return allItems.Count(x => x.CustomerId == customerId);
    }
  }
}
