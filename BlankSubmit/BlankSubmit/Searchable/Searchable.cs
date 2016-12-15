using System.Text.RegularExpressions;
using BlankSubmit.Helpers;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    public abstract class Searchable<T>: IDisplayable where T: class, new()
    {
        protected T This { get; }

        protected Searchable(T thing)
        {
            This = thing ?? new T();
        }

        public override string ToString() => DisplayName;

        public static implicit operator T(Searchable<T> searchable)
        {
            return searchable.This;
        }

        public abstract string DisplayName { get; }
    }
}
