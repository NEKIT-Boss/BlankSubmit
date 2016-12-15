using BlankSubmit.Model;
using Prism.Mvvm;
using Prism.Navigation;

namespace BlankSubmit.ViewModel
{
    class ResultViewModel: BindableBase, INavigationAware
    {
        public Person Person { get; private set; }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            // Do nothing
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            // Do nothing
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Person = parameters[nameof(Person)] as Person;
        }
    }
}
