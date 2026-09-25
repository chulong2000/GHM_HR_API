using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.Resources;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentService _departmentService;
        private readonly IResourceService<GhmHRResource> _ghmHRResource;

        public CompanyService(ICompanyRepository companyRepository,
                              IPositionRepository positionRepository, 
                              IDepartmentService departmentService,
                              IResourceService<GhmHRResource> ghmHRResource)
        {
            _companyRepository = companyRepository;
            _positionRepository = positionRepository;
            _departmentService = departmentService;
            _ghmHRResource = ghmHRResource;
        }

        public async Task<CompanyDetailViewModel> GetDetailCompanyAsync(string tenantId, string companyId)
        {
            var info = new CompanyDetailViewModel
            {
                ListPositions = await _positionRepository.SelectAllPositionActiveAsync(tenantId, companyId),
                ListDepartments = await _departmentService.GetFullTreeActiveAsync(tenantId, companyId)
            };

            return info;
        }

	public async Task<List<CompanysSearchViewModel>> SelectAllAsync(string tenantId)
        {
            return await _companyRepository.SelectAllAsync(tenantId);
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, CompanyMeta companyMeta)
        {
            var companyId = Guid.NewGuid().ToString();

            var isCodeExist = await _companyRepository.CheckExistsCodeAsync(tenantId, companyId, companyMeta.Code?.Trim());
            if (isCodeExist)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString("{0} code: {1} already exists.", _ghmHRResource.GetString("Company"), companyMeta.Code));
            
            var isNameExist = await _companyRepository.CheckExistsNameAsync(tenantId, companyId, companyMeta.Name?.Trim());
            if (isNameExist)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Company"), companyMeta.Name));

            var companys = new Company
            {
                Id = companyId,
                ConcurrencyStamp = companyId,
                Code = companyMeta.Code?.Trim(),
                Name = companyMeta.Name?.Trim(),
                PhoneNumber = companyMeta.PhoneNumber?.Trim(),
                Address = companyMeta.Address?.Trim(),
                Description = companyMeta.Description?.Trim(),
                TaxCode = companyMeta.TaxCode?.Trim(),
                IsActive = companyMeta.IsActive,
                TenantId = tenantId,
                CreateTime = DateTime.Now,
                CreatorId = creatorId,
                CreatorFullName = creatorFullName,
                Logo = companyMeta.Logo?.Trim(),
                LogoFooter = companyMeta.LogoFooter?.Trim()
            };

            var result = await _companyRepository.InsertAsync(companys);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Company")), string.Empty, companyId);
        }


        public async Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, CompanyMeta companyMeta)
        {
            var info = await _companyRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-1, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Company")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

           

            var isNameExit = await _companyRepository.CheckExistsNameAsync(tenantId, id, companyMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-4, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Company"), companyMeta.Name));

            info.Code = companyMeta.Code?.Trim();
            info.Name = companyMeta.Name?.Trim();
            info.PhoneNumber = companyMeta.PhoneNumber?.Trim();
            info.Address = companyMeta.Address?.Trim();
            info.Description = companyMeta.Description?.Trim();
            info.TaxCode = companyMeta.TaxCode?.Trim();
            info.IsActive = companyMeta.IsActive;
            info.ConcurrencyStamp = Guid.NewGuid().ToString();
            info.LastUpdate = DateTime.Now;
            info.LastUpdateUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;
            info.Logo = companyMeta.Logo?.Trim();
            info.LogoFooter = companyMeta.LogoFooter?.Trim();

            var result = await _companyRepository.UpdateAsync(info);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Company")), string.Empty, info.ConcurrencyStamp);
        }


        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id)
        {
            var info = await _companyRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Company")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var isUserExist = await _companyRepository.CheckExistUserAsync(tenantId, info.Id);
            if(isUserExist)
                return new ActionResultResponse(-3, _ghmHRResource.GetString(ErrorMessage.CannotDeleteCompany));

            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;

            var result = await _companyRepository.DeleteAsync(info);

            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Company")));
        }


        public async Task<ActionResultResponse<CompanysDetailViewModel>> GetDetailAsync(string tenantId, string id)
        {
            var info = await _companyRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<CompanysDetailViewModel>(-1, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Company")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<CompanysDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var companysDetail = new CompanysDetailViewModel
            {
                Id = info.Id,
                Code = info.Code,
                Name = info.Name,
                PhoneNumber = info.PhoneNumber,
                Address = info.Address,
                Description = info.Description,
                TaxCode = info.TaxCode,
                IsActive = info.IsActive,
                ConcurrencyStamp = info.ConcurrencyStamp,
                Logo = info.Logo,
                LogoFooter = info.LogoFooter
            };
            return new ActionResultResponse<CompanysDetailViewModel>
            {
                Code = 1,
                Data = companysDetail
            };
        }

        public async Task<List<CompanyUserViewModel>> SelectAllByUserAsync(string tenantId, string userId)
        {
            return await _companyRepository.SelectAllByUserAsync(tenantId, userId);
        }

        public async Task<ActionResultResponse<CompanyLogoViewModel>> GetLogoAsync(string tenantId, string companyId)
        {
            var info = await _companyRepository.GetLogoAsync(tenantId, companyId);
            if (info == null)
                return new ActionResultResponse<CompanyLogoViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists,_ghmHRResource.GetString("Company")));

            var data = new CompanyLogoViewModel
            {
                Logo = info.Logo,
                LogoFooter = info.LogoFooter,
            };

            return new ActionResultResponse<CompanyLogoViewModel>
            {
                Code = 1,
                Data = data
            };
        }
    }
}
