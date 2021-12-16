using ChatApp.Client.ViewModels;
using ChatApp.Client.Views;
using ReactiveUI;
using Splat;

namespace ChatApp.Client;

internal class Bootstrapper
{
    internal Bootstrapper() => ConfigureServices();

    internal void Run()
    {
        var viewModel = new LoginViewModel();
        Locator.CurrentMutable.RegisterConstant(viewModel, typeof(IScreen));

        var view = ViewLocator.Current.ResolveView(viewModel);
        if (view != null)
        {
            view.ViewModel = viewModel;

            Application.Run(view as Form);
        }
    }

    private static void ConfigureServices()
    {
        Locator.CurrentMutable.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>));
    }
}