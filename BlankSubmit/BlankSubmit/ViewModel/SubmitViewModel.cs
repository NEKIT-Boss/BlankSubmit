using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BlankSubmit.Helpers;
using BlankSubmit.Model;
using BlankSubmit.Searchable;
using Nito.AsyncEx;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using VkAPI;

namespace BlankSubmit.ViewModel
{
    class SubmitViewModel: BindableBase, INavigationAware
    {
        /// <summary>
        /// Passed prism navigation service
        /// </summary>
        private readonly INavigationService _navigationService;

        public SubmitViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SubmitCommand = new DelegateCommand(Submit , CanSubmit)
                .ObservesProperty(() => SelectedCity)
                .ObservesProperty(() => SelectedCountry)
                .ObservesProperty(() => SelectedUniversity)
                .ObservesProperty(() => Name)
                .ObservesProperty(() => Surname);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                if (!SetProperty(ref _name, value)) return;
                ValidateSurname();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                if (!SetProperty(ref _surname, value)) return;
                ValidateCountry();
            }
        }

        #region Selectable

        public SearchableCountry SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (!SetProperty(ref _selectedCountry, value)) return;
                ValidateCity();
            }
        }

        public SearchableCity SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                SetProperty(ref _selectedCity, value);
                ValidateUniversity();
            }
        }

        public SearchableUniversity SelectedUniversity
        {
            get { return _selectedUniversity; }
            set { SetProperty(ref _selectedUniversity, value); }
        }

        #endregion

        #region ChainOfValidation

        private void ValidateSurname()
        {
            if (Name != string.Empty) return;

            Surname = string.Empty;
            ValidateCountry();
        }

        private void ValidateCountry()
        {
            if (Surname != string.Empty) return;

            SelectedCountry = null;
            CountryName = string.Empty;
            ValidateCity();
        }

        private void ValidateCity()
        {
            if (SelectedCountry != null) return;

            SelectedCity = null;
            CityName = string.Empty;
            ValidateUniversity();
        }

        private void ValidateUniversity()
        {
            if (SelectedCity != null) return;

            SelectedUniversity = null;
            UniversityName = string.Empty;
        }

        #endregion

        #region Commands

        public ICommand SubmitCommand { get; }

        private void Submit()
        {
            _navigationService.NavigateAsync(NavigationKeys.Result, new NavigationParameters()
            {
                {
                    nameof(Person),
                    new Person
                    {
                        Name = Name,
                        Surname = Surname,
                        Country = SelectedCountry,
                        City = SelectedCity,
                        University = SelectedUniversity
                    }
                }
            });
        }

        private bool CanSubmit()
            => !string.IsNullOrWhiteSpace(Name)
               && !string.IsNullOrWhiteSpace(Surname)
               && SelectedCountry != null
               && SelectedCity != null
               && SelectedUniversity != null;

        #endregion

        #region AsyncLoading

        private List<SearchableCountry> _availableCountries;

        private INotifyTaskCompletion<ILookup<char, SearchableCountry>> _allCountries;
        private INotifyTaskCompletion<List<SearchableCity>> _availableCities;
        private INotifyTaskCompletion<List<SearchableUniversity>> _availableUniversities;

        /// <summary>
        /// Autocompletion delay, to avoid re requesting on property changed event
        /// </summary>
        private static TimeSpan AutoCompleteDelay { get; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// List of availables for suggestion countries
        /// </summary>
        public List<SearchableCountry> AvailableCountries
        {
            get { return _availableCountries; }
            private set { SetProperty(ref _availableCountries, value); }
        }

        /// <summary>
        /// Bindable task completion for indexed lookup of countries
        /// </summary>
        public INotifyTaskCompletion<ILookup<char, SearchableCountry>> AllCountries
        {
            get { return _allCountries; }
            private set { SetProperty(ref _allCountries, value); }
        }

        /// <summary>
        /// Last <see cref="LoadCitiesAsync"/> cancellation source
        /// </summary>
        private CancellationTokenSource _citiesCancellationSource;

        /// <summary>
        /// Task to load cities
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the task</param>
        /// <param name="cityName">parameter to persist <see cref="CityName"/></param>
        /// <returns>Loaded and filtered cities</returns>
        private async Task<List<SearchableCity>> LoadCitiesAsync(CancellationToken cancellationToken, string cityName)
        {
            await Task.Delay(AutoCompleteDelay, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return null;

            return (await VkApi.SearchForCitiesAsync(
                SelectedCountry.Id,
                cityName))
                .Select(x => new SearchableCity(x))
                .ToList();
        }

        /// <summary>
        /// Bindable task completion for available list of cities suggestion
        /// </summary>
        public INotifyTaskCompletion<List<SearchableCity>> AvailableCities
        {
            get { return _availableCities; }
            private set { SetProperty(ref _availableCities, value); }
        }

        /// <summary>
        /// Last <see cref="LoadUniversitiesAsync"/> cancellation source
        /// </summary>
        private CancellationTokenSource _universitiesCancellationSource;

        /// <summary>
        /// Task to load universities
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the task</param>
        /// <param name="universityName"></param>
        /// <returns></returns>
        private async Task<List<SearchableUniversity>> LoadUniversitiesAsync(CancellationToken cancellationToken, string universityName)
        {
            await Task.Delay(AutoCompleteDelay, cancellationToken);
            if (cancellationToken.IsCancellationRequested) return null;

            return (await VkApi.SearchForUniversitiesAsync(
                SelectedCountry.Id, SelectedCity.Id,
                universityName))
                .Select(x => new SearchableUniversity(x))
                .ToList();
        }

        /// <summary>
        /// Bindable task completion for available list of universities suggestion
        /// </summary>
        public INotifyTaskCompletion<List<SearchableUniversity>> AvailableUniversities
        {
            get { return _availableUniversities; }
            private set { SetProperty(ref _availableUniversities, value); }
        }

        #endregion

        private string _name;
        private string _surname;
        private string _countryName;
        private string _cityName;
        private string _universityName;

        private SearchableCountry _selectedCountry;
        private SearchableCity _selectedCity;
        private SearchableUniversity _selectedUniversity;

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                if (!SetProperty(ref _countryName, value)) return;
                if (SelectedCountry?.DisplayName == value) return;
                
                SelectedCountry = null;

                string trimmed = _countryName?.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) 
                    || !AllCountries.Result.Contains(char.ToUpperInvariant(trimmed[0])))
                {
                    AvailableCountries = null;
                    return;
                }

                AvailableCountries = AllCountries.Result[char.ToUpper(trimmed[0])]
                    .Where(x => SearchHelper.ToSearchable(x.DisplayName)
                        .StartsWith(SearchHelper.ToSearchable(_countryName)))
                        .ToList();
            }
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {
                if (!SetProperty(ref _cityName, value)) return;
                if (SelectedCity?.DisplayName == value) return;

                SelectedCity = null;

                if (string.IsNullOrWhiteSpace(_cityName))
                {
                    AvailableCities = null;
                    return;
                }

                _citiesCancellationSource?.Cancel();
                _citiesCancellationSource?.Dispose();

                _citiesCancellationSource = new CancellationTokenSource();

                AvailableCities = NotifyTaskCompletion.Create(LoadCitiesAsync(_citiesCancellationSource.Token, CityName));
            }
        }

        public string UniversityName
        {
            get { return _universityName; }
            set
            {
                if (!SetProperty(ref _universityName, value)) return;
                if (SelectedUniversity?.DisplayName == value) return;
                 
                SelectedUniversity = null;

                if (string.IsNullOrWhiteSpace(value))
                {
                    AvailableUniversities = null;
                    return;
                }

                _universitiesCancellationSource?.Cancel();
                _universitiesCancellationSource?.Dispose();

                _universitiesCancellationSource = new CancellationTokenSource();

                AvailableUniversities = NotifyTaskCompletion.Create(LoadUniversitiesAsync(_universitiesCancellationSource.Token, UniversityName));
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            // Do nothing
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            AllCountries = NotifyTaskCompletion.Create(async () => (await VkApi.GetAllCountriesAsync())
                .Select(x => new SearchableCountry(x))
                .ToLookup(x => char.ToUpperInvariant(x.DisplayName[0])));
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            // Do nothing
        }
    }
}