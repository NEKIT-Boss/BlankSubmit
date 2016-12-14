namespace BlankSubmit.Helpers
{
    public static class SearchHelper
    {
        public static string ToSearchable(string input) => input.ToLower().Trim();
    }
}