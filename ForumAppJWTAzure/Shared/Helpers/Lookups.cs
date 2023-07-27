namespace ForumAppJWTAzure.Shared.Helpers
{
    public static class Lookups
    {
        public class MudPagination
        {
            public const string InfoFormat = "{first_item}-{last_item} of {all_items}";
            public static readonly int[] PageSizeOptions = new int[] { 5, 10, 25, 50, 100, int.MaxValue };
        }

        public static class ToasterText
        {
            public const string YouMustBeLoggedInToVote = "You must be logged in to vote, and you cannot vote for your own post";
        }

        public static class Project
        {
            public const string Client = "Client";
            public const string Server = "Server";
            public const string Shared = "Shared";
        }

        public static class Severity 
        {
            public const string Info = "Info";
            public const string Warning = "Warning";
            public const string Error = "Error";
            public const string Critical = "Critical";
        }
    }
}