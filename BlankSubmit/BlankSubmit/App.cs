using Prism.Unity;
using Xamarin.Forms;

namespace BlankSubmit
{
    public class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null): base(initializer){ }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync($"NavigationPage/{NavigationKeys.Submit}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<SubmitView, SubmitViewModel>(NavigationKeys.Submit);
        }
    }

    public static class NavigationKeys
    {
        public const string Root = "Root";
        public const string Submit = "Submit";
    }
}
