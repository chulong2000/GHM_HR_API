using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Models;
using GHM.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GHM.HR.Domain.IServices
{
    public interface IUserService
    {
        Task<List<UserSearchViewModel>> SelectAllAsync(string tenantId, string companyId, DateTime? contractExpirationDate);
        Task<List<UserSearchAllViewModel>> SelectAllUsersAsync(string tenantId, string companyId);
        Task<List<UserBirthdayViewModel>> SearchUserBirthdayAsync(string tenantId, string companyId, int? month);
        Task<List<UserResignedViewModel>> SearchUserResignedAsync(string tenantId, string companyId, bool isAll, int month);
        Task<List<UserSearchViewModel>> SelectAllUsersByCompanyDepartmentAsync(string tenantId, string companyId,int departmentId);
        Task<List<UserSearchViewModel>> SelectAllUsersByFineFormulaAsync(string tenantId, string companyId, string fineFormulaId);
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, UserMeta userMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, UserMeta userMeta);
        Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id);
        Task<ActionResultResponse<UserDetailViewModel>> GetDetailAsync(string tenantId, string id);
        Task<ActionResultResponse<string>> GetCodeAsync(string tenantId);
        Task<UserProfileViewModel> GetProfileAsync(string tenantId, string id);
        Task<ActionResultResponse<UserCountByRelationshipViewModel>> CountByRelationshipAsync(string companyId, BriefUser currentUser, CancellationToken cancellationToken);
    }
}