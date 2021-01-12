using System;
using System.Windows;
using WiredBrainCoffee.CupOrderAdmin.UI.Startup;

namespace WiredBrainCoffee.CupOrderAdmin.UI
{
  public partial class App : Application
  {
    private static IServiceProvider ServiceProvider { get; set; }

    public static T GetService<T>() where T : class
    {
      if (ServiceProvider == null)
      {
        InitializeServiceProvider();
      }

      return (T)ServiceProvider.GetService(typeof(T));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
      InitializeServiceProvider();
    }

    private static void InitializeServiceProvider()
    {
      var bootstrapper = new Bootstrapper();
      ServiceProvider = bootstrapper.CreateServiceProvider();
    }
  }
}
