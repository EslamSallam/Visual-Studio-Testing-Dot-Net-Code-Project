using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories.Base;

namespace WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories
{

  public class CustomerFileRepository : FileRepositoryBase<Customer>, ICustomerRepository
  {
    public CustomerFileRepository() : base(@"Customers.json")
    {
    }
  }
}
