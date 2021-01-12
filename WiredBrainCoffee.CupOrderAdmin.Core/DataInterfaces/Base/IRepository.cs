using System.Collections.Generic;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Base;

namespace WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces.Base
{
  public interface IRepository<T> where T : IEntity
  {
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task<T> SaveAsync(T entity);
  }
}
