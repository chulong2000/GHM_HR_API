using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Models;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using System.Data;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IMultiCompanyRepository
    {
        Task<bool> CheckExistDoctorCodeAsync(string tenantId, string companyId, string doctorCode);

        Task InsertMultiCompanyAsync(string tenantId, string userId, string creatorId, string creatorFullName, DataTable dataTable);

        Task<List<MultipleCompanySearchViewModel>> GetMultiCompaniesAsync(string tenantId, string companyId,string userId);

        Task<int> ForceDeleteByUserIdAsync(string tenantId, string userId);

        Task<List<MultiCompany>> GetInfoAsync(string tenantId, string userId);

    }
}
