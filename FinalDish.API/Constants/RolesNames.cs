namespace FinalDish.API.Constants
{
    public class RolesNames
    {
        public const string Moderator = "Moderator";
        public const string Admin = "Administrator";

        private static HashSet<string> content =
        [
            Moderator,
            Admin
        ];

        public static IEnumerable<string> Content => content;
    }
}
