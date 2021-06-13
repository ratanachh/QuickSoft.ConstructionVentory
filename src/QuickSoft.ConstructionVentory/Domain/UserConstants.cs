namespace QuickSoft.ConstructionVentory.Domain
{
    public static class UserConstants
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Developer = nameof(Developer);

        public static string GetUserTypeString(int userType)
        {
            return userType switch
            {
                0 => User,
                1 => Admin,
                2 => Developer,
                _ => userType.ToString()
            };
        }
    }
}