using System.Collections;
using System.ComponentModel;
using BlankSubmit.Searchable;
using Xamarin.Forms;

namespace BlankSubmit
{
    public partial class AutoCompleteBox: ContentView
    {
        public AutoCompleteBox()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoCompleteBox), default(string), BindingMode.TwoWay);

        /// <summary>
        /// Autocomplete text
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteBox), string.Empty);

        /// <summary>
        /// Placeholder text
        /// </summary>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty SuggestionsProperty =
            BindableProperty.Create(nameof(Suggestions), typeof(IEnumerable), typeof(AutoCompleteBox));

        /// <summary>
        /// List of provided suggestions
        /// </summary>
        public IEnumerable Suggestions
        {
            get { return (IEnumerable)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteBox), null, BindingMode.TwoWay);

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty
            = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoCompleteBox), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty SuggestionsHeightRequestProperty 
            = BindableProperty.Create(nameof(SuggestionsHeightRequest), typeof(double), typeof(AutoCompleteBox), 300.0);

        public double SuggestionsHeightRequest
        {
            get { return (double)GetValue(SuggestionsHeightRequestProperty); }
            set { SetValue(SuggestionsHeightRequestProperty, value); }
        }

        public static readonly BindableProperty IsSuggestionsListOpenProperty
            = BindableProperty.Create(nameof(IsSuggestionsListOpen), typeof(bool), typeof(AutoCompleteBox), false);

        public bool IsSuggestionsListOpen
        {
            get { return (bool)GetValue(IsSuggestionsListOpenProperty); }
            private set { SetValue(IsSuggestionsListOpenProperty, value); }
        }

        private void SearchEntry_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var searchBar = sender as SearchBar;
            if (searchBar == null) return;

            if (e.PropertyName == nameof(IsFocused) || e.PropertyName == nameof(Text))
            {
                IsSuggestionsListOpen = !string.IsNullOrWhiteSpace(searchBar.Text)
                                        && searchBar.IsFocused;
            }
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Text = (e.SelectedItem as IDisplayable)?.DisplayName ?? e.SelectedItem.ToString();
        }
    }
}
