namespace QuickSoft.ConstructionVentory.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(string username, string userType, string auditId);
        public IJwtTokenGenerator ValidTokenTime(int second);
    }
}