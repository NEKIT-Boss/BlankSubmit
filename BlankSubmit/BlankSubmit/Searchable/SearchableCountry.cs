using BlankSubmit.Helpers;
using VkAPI.Countries;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    class SearchableCountry : IAutoCompleteSearchObject
    {
        private readonly Country _country;

        public SearchableCountry(Country country)
        {
            _country = country;
        }

        public override string ToString()
        {
            return _country.Name;
        }

        public string Name => _country.Name;
        public string SearchableName => SearchHelper.ToSearchable(Name);
        public int Id => _country.Id;

        public string StringToSearchBy()
        {
            return ToString();
        }
    }
}