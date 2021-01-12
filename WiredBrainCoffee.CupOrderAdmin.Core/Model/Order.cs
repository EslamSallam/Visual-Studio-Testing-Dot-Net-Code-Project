using WiredBrainCoffee.CupOrderAdmin.Core.Model.Base;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Enums;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Model
{
  public class Order : IEntity
  {
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public OrderStatus Status { get; set; }

    public double DiscountInPercent { get; set; }
  }
}
