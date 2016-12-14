using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BlankSubmit.Searchable;
using Nito.AsyncEx;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using VkAPI;

namespace BlankSubmit
{
    class SubmitViewModel: BindableBase, INavigationAware
    {
        private string _name;
        private string _surname;
        private string _cityName;
        private string _countryName;
        private string _universityName;

        private SearchableCountry _selectedCountry;
        private SearchableCity _selectedCity;
        private SearchableUniversity _selectedUniversity;

        private INotifyTaskCompletion<List<SearchableCountry>> _availableCountries;
        private INotifyTaskCompletion<List<SearchableCity>> _availableCities;
        private INotifyTaskCompletion<List<SearchableUniversity>> _availableUniversities;

        public SubmitViewModel()
        {
            CountrySelectedCommand = new DelegateCommand<SearchableCountry>(x => SelectedCountry = x);
            CitySelectedCommand = new DelegateCommand<SearchableCity>( x => SelectedCity = x);

            SubmitCommand = new DelegateCommand(() => {}, () => (SelectedCity != null ) && (SelectedCountry != null))
                .ObservesProperty(() => SelectedCity).ObservesProperty(() => SelectedCountry);
        }

        public ICommand SubmitCommand { get; }
        public DelegateCommand<SearchableCity> CitySelectedCommand { get; }
        public DelegateCommand<SearchableCountry> CountrySelectedCommand { get; }

        public INotifyTaskCompletion<List<SearchableCity>> AvailableCities
        {
            get { return _availableCities; }
            private set { SetProperty(ref _availableCities, value); }
        }

        public INotifyTaskCompletion<List<SearchableCountry>> AvailableCountries
        {
            get { return _availableCountries; }
            private set { SetProperty(ref _availableCountries, value); }
        }

        public INotifyTaskCompletion<List<SearchableUniversity>> AvailableUniversities
        {
            get { return _availableUniversities; }
            set { SetProperty(ref _availableUniversities, value); }
        }

        public SearchableCity SelectedCity
        {
            get { return _selectedCity; }
            private set { SetProperty(ref _selectedCity, value); }
        }

        public SearchableCountry SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry,value); }
        }

        public SearchableUniversity SelectedUniversity
        {
            get { return _selectedUniversity; }
            set { SetProperty( ref _selectedUniversity, value); }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                SetProperty(ref _surname, value);
            }
        }

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                SetProperty(ref _countryName, value);
                if (SelectedCountry?.Name != value)
                {
                    SelectedCountry = null;
                }
            }
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {
                SetProperty(ref _cityName, value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    AvailableCities = NotifyTaskCompletion.Create(async () =>
                        {
                            return (await VkApi.SearchForCitiesAsync(
                                SelectedCountry.Id,
                                CityName))
                                .Select(x => new SearchableCity(x))
                                .ToList();
                        }); 
                }
            }
        }

        public string UniversityName
        {
            get { return _universityName; }
            set
            {
                SetProperty(ref _universityName, value);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    AvailableUniversities = NotifyTaskCompletion.Create(async () =>
                    {
                        return (await VkApi.SearchForUniversitiesAsync(SelectedCountry.Id, SelectedCity.Id, UniversityName))
                            .Select(x => new SearchableUniversity(x))
                            .ToList();
                    });
                }
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            // Do nothing
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            AvailableCountries = NotifyTaskCompletion.Create(async () => (await VkApi.GetAllCountriesAsync())
                .Select(x => new SearchableCountry(x))
                .ToList());
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            // Do nothing
        }
    }
}