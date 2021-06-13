namespace QuickSoft.ConstructionVentory.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 10;
        private const char BCryptMinorRevision = 'y';

        public string Hash(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(WorkFactor, BCryptMinorRevision);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
        
        public bool Verify(string password, string encryptedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, encryptedPassword);
        }
    }
}