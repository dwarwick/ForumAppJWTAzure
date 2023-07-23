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
    }
}