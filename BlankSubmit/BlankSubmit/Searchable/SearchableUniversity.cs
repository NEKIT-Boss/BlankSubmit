﻿using VkAPI.Universities;

namespace BlankSubmit.Searchable
{
    class SearchableUniversity : Searchable<University>
    {
        public int Id => This.Id;
        public string Name => This.Name;

        public override string DisplayName => Name;

        public SearchableUniversity(University thing) : base(thing)
        {
        }
    }
}