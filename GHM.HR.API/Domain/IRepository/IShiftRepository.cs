using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GHM.HR.API.Domain.IRepository
{
    
    public interface IShiftRepository 
    {
        Task<List<ShiftsViewModel>> SelectAllAsync(string tenantId, string companyId);
        Task<int> InsertAsync(Shift shift);
        Task<int> UpdateAsync(Shift shift);
        Task<int> DeleteAsync(Shift shift);
        Task<Shift> GetInfoAsync(string id);
        Task<bool> checkNameExists(string tenantId, string companyId, string name, string shiftId);
    }
}
