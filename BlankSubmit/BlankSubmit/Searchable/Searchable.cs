using BlankSubmit.Helpers;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    public abstract class Searchable<T> where T: class, new()
    {
        protected T This { get; }

        protected Searchable(T thing)
        {
            This = thing ?? new T();
        }

        public string SearchIndex => SearchHelper.ToSearchable(ToString());

        public static implicit operator T(Searchable<T> searchable)
        {
            return searchable.This;
        }
    }
}
