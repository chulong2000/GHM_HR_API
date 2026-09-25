using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.IServices
{
    public interface IPositionService
    {
        Task<List<PositionSearchViewModel>> SelectAllAsync(string tenantId,string companyId);
        Task<List<PositionSearchViewModel>> SelectAllPositionActiveAsync(string tenantId, string companyId);
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, PositionMeta positionMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, PositionMeta positionMeta);
        Task<ActionResultResponse> DeleteAsync(string tenantId,string deleteUserId, string deleteFullName, string deleteAvatar, string id);
        Task<ActionResultResponse<PositionDetailViewModel>> GetDetailAsync(string tenantId, string id);
        Task<ActionResultResponse<string>> UpdateIsActive(string companyId, string id, bool isActive);
    }
}
