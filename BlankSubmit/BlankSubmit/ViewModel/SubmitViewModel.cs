using System;
using System.Collections.Generic;
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

            CountrySelectedCommand = new DelegateCommand<SearchableCountry>(x =>
            {
                SelectedCountry = x;
            });
        }

        public DelegateCommand<SearchableCountry> CountrySelectedCommand { get; }

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

        public SearchableCountry SelectedCountry
        {
            get;
            set;
        }
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
        public INotifyTaskCompletion<List<SearchableCountry>> AvailableCountries { get; private set; }
        public INotifyTaskCompletion<List<SearchableUniversity>> AvailableUniversities { get; private set; }

        private string _countryName;
        private string _cityName;
        private string _universityName;

        private CancellationTokenSource _citiesCancellationSource;
        private CancellationToken _citiesCancellationToken;

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                _countryName = value;

                if (SelectedCountry?.ToString() != value)
                {
                    SelectedCountry = null;

                    SelectedCity = null;
                    CityName = string.Empty;

                    SelectedUniversity = null;
                    UniversityName = string.Empty;
                }

            }
        }

        public string CityName
        {
            get { return _cityName; }
            set
            {
                _cityName = value;

                if (SelectedCity?.SearchIndex != value)
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
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
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

                if (SelectedUniversity?.SearchIndex != SearchHelper.ToSearchable(value))
                {
                    SelectedUniversity = null;
                }

                if (string.IsNullOrWhiteSpace(value)) return;

                AvailableUniversities = NotifyTaskCompletion.Create(async () =>
                {
                    return (await VkApi.SearchForUniversitiesAsync(SelectedCountry.Id, SelectedCity.Id, UniversityName))
                        .Select(x => new SearchableUniversity(x))
                        .ToList();
                });
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