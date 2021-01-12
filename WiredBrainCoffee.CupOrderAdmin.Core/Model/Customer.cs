using WiredBrainCoffee.CupOrderAdmin.Core.Model.Enums;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Base;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Model
{
  public class Customer : IEntity
  {
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public CustomerMembership Membership { get; set; }
  }
}
