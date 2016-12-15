using System.Text.RegularExpressions;
using BlankSubmit.Helpers;
using XLabs.Forms.Controls;

namespace BlankSubmit.Searchable
{
    /// <summary>
    /// Common wrapper to enable searching and separation from DataLayer
    /// Could have used mapper, but why, for this kind of small example
    /// </summary>
    /// <typeparam name="T">Wrapee</typeparam>
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
