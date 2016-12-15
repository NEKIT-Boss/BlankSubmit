using BlankSubmit.Model;
using Prism.Mvvm;
using Prism.Navigation;

namespace BlankSubmit.ViewModel
{
    class ResultViewModel: BindableBase, INavigationAware
    {
        private Person _person;

        public Person Person
        {
            get { return _person; }
            private set { SetProperty(ref _person, value); }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            // Do nothing
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            // Do nothing
        }

        // Cathing the parameters from another view, before the page load
        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Person = parameters[nameof(Person)] as Person;
        }
    }
}
