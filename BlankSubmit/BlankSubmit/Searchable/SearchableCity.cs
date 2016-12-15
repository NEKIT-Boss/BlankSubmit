using VkAPI.Cities;

namespace BlankSubmit.Searchable
{
    class SearchableCity : Searchable<City>
    {
        public int Id => This.Id;
        public string Name => This.Name;

        public override string ToString() => Name;

        public SearchableCity(City thing) : base(thing)
        {
        }
    }
}