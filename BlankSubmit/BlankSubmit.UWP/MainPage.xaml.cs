using Microsoft.Practices.Unity;
using Prism.Unity;

namespace BlankSubmit.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new BlankSubmit.App(new UwpInitializer()));
        }

        public class UwpInitializer : IPlatformInitializer

        {
            public void RegisterTypes(IUnityContainer container)
            {

            }
        }
    }
}
