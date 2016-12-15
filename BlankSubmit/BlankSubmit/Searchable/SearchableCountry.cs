using BlankSubmit.Helpers;
using VkAPI.Countries;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    class SearchableCountry : Searchable<Country>
    {
        public int Id => This.Id;
        public string Name => This.Name;

        public override string ToString() => Name;

        public SearchableCountry(Country thing) : base(thing)
        {
        }
    }
}