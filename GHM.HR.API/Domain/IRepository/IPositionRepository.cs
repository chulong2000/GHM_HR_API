using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.IRepository
{
    public interface IPositionRepository
    {
        Task<List<PositionSearchViewModel>> SelectAllAsync(string tenantId,string companyId);
        Task<List<PositionSearchViewModel>> SelectAllPositionActiveAsync(string tenantId,string companyId);
        Task<int> InsertAsync(Position position);
        Task<int> UpdateAsync(Position position);
        Task<int> DeleteAsync(Position position);
        Task<int> ForceDeleteAsync(string companyId,string id);
        Task<Position> GetInfoAsync(string id);
        Task<bool> CheckExistNameAsync(string tenantId,string companyId,string name);
        Task<bool> CheckExistCodeAsync(string tenantId, string companyId, string code,string id);
        Task<bool> CheckExistByNameAsync(string tenantId,string companyId,string id,string name);
        Task<int> UpdateByPositionNameAsync(string tenantId, string companyId,string positionId, string positionName);
        Task<bool> CheckExistsAsync(string companyId,string id);
        Task<int> UpdateIsActive(string companyId,string id, bool isActive);
    }
}
