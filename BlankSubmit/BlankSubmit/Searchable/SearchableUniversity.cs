using BlankSubmit.Helpers;
using VkAPI.Universities;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    class SearchableUniversity : IAutoCompleteSearchObject
    {
        private readonly University _university;

        public SearchableUniversity(University university)
        {
            _university = university;
        }

        public int Id => _university.Id;
        public string Name => _university.Name;
        public string SearchableName => SearchHelper.ToSearchable(Name);

        public override string ToString()
        {
            return _university.Name;
        }

        public string StringToSearchBy()
        {
            return ToString();
        }
    }
}