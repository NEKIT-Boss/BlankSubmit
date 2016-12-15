using Prism.Mvvm;
using VkAPI.Cities;
using VkAPI.Countries;
using VkAPI.Universities;

namespace BlankSubmit.Model
{
    class Person : BindableBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Surname} {Name}";

        /// <summary>
        /// DataLayer reference is usually bad, but for the testing demo will work perfectly fine, 
        /// taking in account, that wrapping will not do any good things.
        /// </summary>s
        public Country Country { get; set; }

        /// <summary>
        /// DataLayer reference is usually bad, but for the testing demo will work perfectly fine, 
        /// taking in account, that wrapping will not do any good things.
        /// </summary>
        public City City { get; set; }

        /// <summary>
        /// DataLayer reference is usually bad, but for the testing demo will work perfectly fine, 
        /// taking in account, that wrapping will not do any good things.
        /// </summary>
        public University University { get; set; }
    }
}
