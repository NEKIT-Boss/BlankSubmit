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
using PropertyChanged;
using VkAPI;

namespace BlankSubmit.ViewModel
{
    class SubmitViewModel: BindableBase, INavigationAware
    {
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

        #region Selections

        public string Name { get; set; }
        public string Surname { get; set; }

        public SearchableCity SelectedCity { get; set; }

        public SearchableCountry SelectedCountry { get; set; }
        public SearchableUniversity SelectedUniversity { get; set; }

        #endregion

        #region Commands

        public ICommand SubmitCommand { get; }

        private bool CanSubmit()
            => !string.IsNullOrWhiteSpace(Name)
               && !string.IsNullOrWhiteSpace(Surname)
               && SelectedCountry != null
               && SelectedCity != null
               && SelectedUniversity != null;

        #endregion

        public INotifyTaskCompletion<List<SearchableCity>> AvailableCities { get; private set; }
        public INotifyTaskCompletion<ILookup<char, SearchableCountry>> AllCountries { get; private set; }
        public INotifyTaskCompletion<List<SearchableUniversity>> AvailableUniversities { get; private set; }

        public List<SearchableCountry> AvailableCountries { get; private set; }

        private string _countryName;
        private string _cityName;
        private string _universityName;

        private static TimeSpan AutoCompleteDelay { get; } = TimeSpan.FromMilliseconds(100);

        private CancellationToken _citiesCancellationToken;
        private CancellationTokenSource _citiesCancellationSource;

        private CancellationToken _universitiesCancellationToken;
        private CancellationTokenSource _universitiesCancellationSource;

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;

                if (SelectedCountry?.DisplayName != value)
                {
                    SelectedCountry = null;

                    SelectedCity = null;
                    CityName = string.Empty;

                    SelectedUniversity = null;
                    UniversityName = string.Empty;
                }

                string trimmed = _countryName?.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || !AllCountries.Result.Contains(char.ToUpper(trimmed[0])))
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
                _cityName = value;

                if (SelectedCity?.DisplayName != value)
                {
                    SelectedCity = null;
                    UniversityName = string.Empty;
                }

                if (string.IsNullOrWhiteSpace(_cityName))
                {
                    AvailableCities = null;
                    return;
                }

                _citiesCancellationSource?.Cancel();
                _citiesCancellationSource?.Dispose();

                _citiesCancellationSource = new CancellationTokenSource();
                _citiesCancellationToken = _citiesCancellationSource.Token;

                AvailableCities = NotifyTaskCompletion.Create(LoadCitiesAsync(_citiesCancellationToken, CityName));
            }
        }

        private async Task<List<SearchableCity>> LoadCitiesAsync(CancellationToken cancellationToken, string cityName)
        {
            await Task.Delay(AutoCompleteDelay, cancellationToken);
            if(cancellationToken.IsCancellationRequested) return null;
            
            return (await VkApi.SearchForCitiesAsync(
                SelectedCountry.Id,
                cityName))
                .Select(x => new SearchableCity(x))
                .ToList();
        }

        public string UniversityName
        {
            get { return _universityName; }
            set
            {
                _universityName = value;

                if (SelectedUniversity?.DisplayName != SearchHelper.ToSearchable(value))
                {
                    SelectedUniversity = null;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    AvailableUniversities = null;
                    return;
                }

                _universitiesCancellationSource?.Cancel();
                _universitiesCancellationSource?.Dispose();

                _universitiesCancellationSource = new CancellationTokenSource();
                _universitiesCancellationToken = _universitiesCancellationSource.Token;

                AvailableUniversities = NotifyTaskCompletion.Create(LoadUniversitiesAsync(_universitiesCancellationToken, UniversityName));
            }
        }

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