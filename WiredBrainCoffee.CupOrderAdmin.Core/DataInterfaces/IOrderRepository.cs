using System.Collections.Generic;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces.Base;

namespace WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces
{
  public interface IOrderRepository : IRepository<Order>
  {
    Task<IEnumerable<Order>> GetAllByCustomerIdAsync(int customerId);

    Task<int> GetCountByCustomerIdAsync(int customerId);
  }
}
