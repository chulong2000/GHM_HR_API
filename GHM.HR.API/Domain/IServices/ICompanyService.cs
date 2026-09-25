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
    public interface ICompanyService
    {
        Task<CompanyDetailViewModel> GetDetailCompanyAsync(string tenantId, string companyId);
        Task<List<CompanyUserViewModel>> SelectAllByUserAsync(string tenantId, string userId);
        Task<List<CompanysSearchViewModel>> SelectAllAsync(string tenantId);
		Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, CompanyMeta companyMeta);
		Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id,CompanyMeta companyMeta);
		Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id);
		Task<ActionResultResponse<CompanysDetailViewModel>> GetDetailAsync(string tenantId, string id);
        Task<ActionResultResponse<CompanyLogoViewModel>> GetLogoAsync(string tenantId, string companyId);
    }
}
