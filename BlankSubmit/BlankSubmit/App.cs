using BlankSubmit.ViewModel;
using Prism.Unity;
using Xamarin.Forms;

namespace BlankSubmit
{
    public class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null): base(initializer){ }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync($"{NavigationKeys.Root}/{NavigationKeys.Submit}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>(NavigationKeys.Root);

            // A bit of coupling for a bit of performance avoiding the ViewModelLocator use
            Container.RegisterTypeForNavigation<View.SubmitView, SubmitViewModel>(NavigationKeys.Submit);
            Container.RegisterTypeForNavigation<View.ResultView, ResultViewModel>(NavigationKeys.Result);
        }
    }

    public static class NavigationKeys
    {
        public const string Root = "Root";
        public const string Submit = "Submit";
        public const string Result = "Result";
    }
}
