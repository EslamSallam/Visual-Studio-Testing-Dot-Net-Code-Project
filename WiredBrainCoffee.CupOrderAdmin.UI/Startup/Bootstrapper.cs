using Microsoft.Extensions.DependencyInjection;
using System;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;
using WiredBrainCoffee.CupOrderAdmin.DataAccess.Repositories;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel;
using WiredBrainCoffee.CupOrderAdmin.UI.ViewModel.Base;

namespace WiredBrainCoffee.CupOrderAdmin.UI.Startup
{
  public class Bootstrapper
  {
    public IServiceProvider CreateServiceProvider()
    {
      var serviceCollection = new ServiceCollection();

      ConfigureServices(serviceCollection);

      return serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<ICustomerRepository, CustomerFileRepository>();
      services.AddTransient<IOrderRepository, OrderFileRepository>();
      services.AddTransient<ICoffeeCupRepository, CoffeeCupFileRepository>();
      services.AddTransient<IOrderCreationService, OrderCreationService>();
      services.AddTransient<TabViewModelBase, CustomerViewModel>();
      services.AddTransient<TabViewModelBase, CoffeeCupViewModel>();
      services.AddTransient<MainViewModel>();
    }
  }
}
