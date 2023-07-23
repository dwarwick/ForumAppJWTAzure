namespace ForumAppJWTAzure.Shared.Helpers
{
    public static class StringHelper
    {
        public static string GetLocalDate(DateTime? dateTime)
        {
            DateTime? date = dateTime?.ToLocalTime();
            return date?.ToShortDateString() ?? string.Empty;
        }
    }
}
