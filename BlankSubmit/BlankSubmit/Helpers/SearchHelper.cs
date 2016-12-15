namespace BlankSubmit.Helpers
{
    /// <summary>
    /// SearchService, just an example
    /// Could have even been the service, to improve testability and enable DI
    /// Just to show, that I know
    /// </summary>
    public static class SearchHelper
    {
        public static string ToSearchable(string input) => input.ToLower().Trim();
    }
}