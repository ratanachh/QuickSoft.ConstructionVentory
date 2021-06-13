namespace QuickSoft.ConstructionVentory.Infrastructure.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string encryptedPassword);
    }
}