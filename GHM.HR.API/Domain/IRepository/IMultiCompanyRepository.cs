using GHM.HR.API.Domain.Models;
using GHM.HR.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IMultiCompanyRepository
    {
        Task<bool> CheckExistDoctorCodeAsync(string tenantId, string companyId, string doctorCode);

        Task<List<string>> InsertBulkAsync(List<MultiCompany> users);
    }
}
