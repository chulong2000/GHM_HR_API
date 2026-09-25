using GHM.HR.API.Domain.Resources;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Infrastructure.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IResourceService<GhmHRResource> _ghmHRResource;
        public PositionService(IPositionRepository positionRepository,
                               IResourceService<GhmHRResource> ghmHRResource)
        {
            _positionRepository = positionRepository;
            _ghmHRResource = ghmHRResource;

        }
        public async Task<List<PositionSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            return await _positionRepository.SelectAllAsync(tenantId, companyId);
        }

        public async Task<List<PositionSearchViewModel>> SelectAllPositionActiveAsync(string tenantId, string companyId)
        {
            return await _positionRepository.SelectAllPositionActiveAsync(tenantId, companyId);
        }


        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, PositionMeta positionMeta)
        {
            var positionId = Guid.NewGuid().ToString();
            var isCodeExit = await _positionRepository.CheckExistCodeAsync(tenantId, positionMeta.CompanyId, positionMeta.Code?.Trim(), positionId);
            if (isCodeExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Position"), positionMeta.Code?.Trim()));

            var isNameExit = await _positionRepository.CheckExistNameAsync(tenantId, positionMeta.CompanyId, positionMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Position"), positionMeta.Name?.Trim()));


            var result = await _positionRepository.InsertAsync(
                new Position
                {
                    Id = positionId,
                    CompanyId = positionMeta.CompanyId,
                    ConcurrencyStamp = positionId,
                    Code = positionMeta.Code,
                    Name = positionMeta.Name,
                    Description = positionMeta.Description,
                    IsMultiple = false,
                    IsActive = positionMeta.IsActive,
                    TenantId = tenantId,
                    CreateTime = DateTime.Now,
                    CreatorId = creatorId,
                    CreatorFullName = creatorFullName
                }
            );

            if(result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));
            
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Position")), string.Empty, positionId);

        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, PositionMeta positionMeta)
        {
            var info = await _positionRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Position")));

            if (info.CompanyId != positionMeta.CompanyId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var isCodeExist = await _positionRepository.CheckExistCodeAsync(tenantId, positionMeta.CompanyId, positionMeta.Code, id);
            if(isCodeExist)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Position"), positionMeta.Code));

            var isNameExit = await _positionRepository.CheckExistByNameAsync(tenantId, positionMeta.CompanyId, id, positionMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Position"), positionMeta.Name));

            var oldName = info.Name;
            info.CompanyId = positionMeta.CompanyId;
            info.Code = positionMeta.Code;
            info.Name = positionMeta.Name;
            info.Description = positionMeta.Description;
            info.IsActive = positionMeta.IsActive;
            info.ConcurrencyStamp = Guid.NewGuid().ToString();
            info.LastUpdate = DateTime.Now;
            info.LastUpdatedUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;

            var result = await _positionRepository.UpdateAsync(info);
          
            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            if (oldName != info.Name)
            {
                await _positionRepository.UpdateByPositionNameAsync(tenantId, info.CompanyId, info.Id, info.Name);
            }
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Position")), string.Empty, info.ConcurrencyStamp);

        }

        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id)
        {
            var info = await _positionRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Position")));

            if(info.TenantId != tenantId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            if (info.IsMultiple == true)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            //check duoc su dung boi user
           
            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;
            var result = await _positionRepository.DeleteAsync(info);
            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Position")));
        }

        public async Task<ActionResultResponse<PositionDetailViewModel>> GetDetailAsync(string tenantId, string id)
        {
            var info = await _positionRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<PositionDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Position")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<PositionDetailViewModel>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));


            var positionDetail = new PositionDetailViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                Code = info.Code,
                Name = info.Name,
                Description = info.Description,
                IsMultiple = info.IsMultiple,
                IsActive = info.IsActive,
                ConcurrencyStamp = info.ConcurrencyStamp,
            };
            return new ActionResultResponse<PositionDetailViewModel>
            {
                Code = 1,
                Data = positionDetail
            };
        }

        public async Task<ActionResultResponse<string>> UpdateIsActive(string companyId, string id, bool isActive)
        {
            var checkExits = await _positionRepository.CheckExistsAsync(companyId,id);
            if (!checkExits)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("idPositon")));
            var result = await _positionRepository.UpdateIsActive(companyId,id, isActive);
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Positon")));
        }

    }
}
