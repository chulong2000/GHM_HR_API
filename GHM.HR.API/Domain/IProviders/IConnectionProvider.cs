namespace GHM.HR.API.Domain.IProviders
{
    public interface IConnectionProvider
    {
        Task<string> GetConnectionStringAsync(string companyId);
    }
}
