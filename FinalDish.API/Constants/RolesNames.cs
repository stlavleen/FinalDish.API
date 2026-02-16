namespace FinalDish.API.Constants
{
    public class RolesNames
    {
        public const string Moderator = "Moderator";
        public const string Admin = "Administrator";

        private static string[] content =
        [
            Moderator,
            Admin
        ];

        public static IEnumerable<string> Content => content;
    }
}
