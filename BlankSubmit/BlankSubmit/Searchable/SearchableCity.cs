using BlankSubmit.Helpers;
using VkAPI.Cities;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    class SearchableCity : IAutoCompleteSearchObject
    {
        private readonly City _city;

        public SearchableCity(City city)
        {
            _city = city;
        }

        public int Id => _city.Id;
        public string Name => _city.Name;
        public string SearchableName => SearchHelper.ToSearchable(Name);

        public override string ToString()
        {
            return _city.Name;
        }

        public string StringToSearchBy()
        {
            return ToString();
        }
    }
}