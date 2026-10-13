using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IShiftService
    {
        Task<List<ShiftsViewModel>> SelectAllByCompanyIdAsync(string tenantId, string companyId);            
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, ShiftMeta shiftMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string updateId, string updateFullName, ShiftMeta shiftMeta, string shiftId, string companyId, string tenantId);
        Task<ActionResultResponse> DeleteAsync(string Id);
        Task<ActionResultResponse<ShiftsViewModel>> GetDetailAsync(string id);
    }
}
