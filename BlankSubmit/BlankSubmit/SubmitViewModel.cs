using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VkAPI;
using VkAPI.Countries;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Mvvm;

namespace BlankSubmit
{
    class SubmitViewModel: ViewModel
    {
        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set { SetProperty(ref _countries, value); }
        }

        private IList<Country> _availableCountries;
        private string _name;
        private string _surname;
        private string _cityName;
        private string _countryName;
        private string _universityName;
        private ObservableCollection<Country> _countries;

        public SubmitViewModel()
        {
            LoadCountriesCommand = new Command(async () =>
            {
                _availableCountries = await VkApi.GetAllCountriesAsync();
                _countries = new ObservableCollection<Country>(_availableCountries);
            });

            SubmitCommand = new RelayCommand(() => { });
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Surname
        {
            get { return _surname; }
            set { SetProperty(ref _surname, value); }
        }

        public string CountryName
        {
            get { return _countryName; }
            set
            {
                SetProperty(ref _countryName, value);
                if (_availableCountries != null)
                {
                    Countries = new ObservableCollection<Country>(
                        _availableCountries
                            .Where(x => x.Name.ToLower().StartsWith(_countryName.ToLower())
                            ));    
                }
            }
        }

        public string CityName
        {
            get { return _cityName; }
            set { SetProperty(ref _cityName, value); }
        }

        public string UniversityName
        {
            get { return _universityName; }
            set { SetProperty(ref _universityName, value); }
        }

        public ICommand SubmitCommand { get; }
        public ICommand LoadCountriesCommand { get; }
    }
}
