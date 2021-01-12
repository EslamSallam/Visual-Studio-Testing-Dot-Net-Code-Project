using WiredBrainCoffee.CupOrderAdmin.Core.Model.Base;

namespace WiredBrainCoffee.CupOrderAdmin.Core.Model
{
  public class CoffeeCup : IEntity
  {
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string SerialCode => $"CoffeeCup_{Id}";
  }
}
