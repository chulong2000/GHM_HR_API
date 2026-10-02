using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.Resources;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.IRepository;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Infrastructure.Service
{
    public class TitleService : ITitleService
    {
        private readonly ITitleRepository _titleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResourceService<GhmHRResource> _ghmHRResource;
        public TitleService(ITitleRepository titleRepository, IUserRepository userRepository, IResourceService<GhmHRResource> ghmHRResource)
        {
            _titleRepository = titleRepository;
            _ghmHRResource = ghmHRResource;
            _userRepository = userRepository;
        }

        public async Task<List<TitleSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            return await _titleRepository.SelectAllAsync(tenantId, companyId);
        }

        public async Task<List<TitleSearchViewModel>> SelectAllTitleActiveAsync(string tenantId, string companyId)
        {
            return await _titleRepository.SelectAllTitleActiveAsync(tenantId, companyId);
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, TitleMeta titleMeta)
        {
            var titleId = Guid.NewGuid().ToString();
            var isNameExit = await _titleRepository.CheckExistsNameAsync(tenantId, titleMeta.CompanyId, titleMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Title"), titleMeta.Name));

            var isCodeExit = await _titleRepository.CheckExistsCodeAsync(tenantId, titleMeta.CompanyId, titleMeta.Code?.Trim(), titleId);
            if (isCodeExit)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Title"), titleMeta.Code));

            var result = await _titleRepository.InsertAsync(new Title
            {
                Id = titleId,
                CompanyId = titleMeta.CompanyId,
                ConcurrencyStamp = titleId,
                Code = titleMeta.Code?.Trim(),
                Name = titleMeta.Name?.Trim(),
                Description = titleMeta.Description?.Trim(),
                IsActive = titleMeta.IsActive,
                TenantId = tenantId,
                CreateTime = DateTime.Now,
                CreatorId = creatorId,
                CreatorFullName = creatorFullName
            });

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Title")), string.Empty, titleId);
        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, TitleMeta titleMeta)
        {
            var info = await _titleRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Title")));

            if (info.TenantId != tenantId || info.CompanyId != titleMeta.CompanyId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            if (info.ConcurrencyStamp != titleMeta.ConcurrencyStamp)
                return new ActionResultResponse<string>(-4, _ghmHRResource.GetString(ErrorMessage.AlreadyUpdatedByAnother));

            var isCodeExist = await _titleRepository.CheckExistsCodeAsync(tenantId, titleMeta.CompanyId, titleMeta.Code, id);
            if (isCodeExist)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Title"), titleMeta.Code));

            var isNameExit = await _titleRepository.CheckExistByNameAsync(tenantId, titleMeta.CompanyId, id, titleMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Title"), titleMeta.Name));

            var oldName = info.Name;
            info.CompanyId = titleMeta.CompanyId;
            info.Code = titleMeta.Code;
            info.Name = titleMeta.Name;
            info.Description = titleMeta.Description;
            info.IsActive = titleMeta.IsActive;
            info.ConcurrencyStamp = Guid.NewGuid().ToString();
            info.LastUpdate = DateTime.Now;
            info.LastUpdateUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;

            var result = await _titleRepository.UpdateAsync(info);
            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            if (oldName != info.Name)
                await _titleRepository.UpdateByTitleNameAsync(tenantId, info.CompanyId, info.Id, info.Name);

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Title")), string.Empty, info.ConcurrencyStamp);
        }
        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id)
        {
            var info = await _titleRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Title")));
            if (info.TenantId != tenantId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            //check duoc su dung boi user
            var isUsedByUser = await _userRepository.CheckExistsTitleByUserAsync(tenantId, id);
            if (isUsedByUser)
                return new ActionResultResponse(-6, _ghmHRResource.GetString(ErrorMessage.CannotDelete, _ghmHRResource.GetString("Title"), _ghmHRResource.GetString("User")));

            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;
            var result = await _titleRepository.DeleteAsync(info);
            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Title")));
        }

        public async Task<ActionResultResponse<TitleDetailViewModel>> GetDetailAsync(string tenantId, string id)
        {
            var info = await _titleRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<TitleDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Title")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<TitleDetailViewModel>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var titleDetail = new TitleDetailViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                Code = info.Code,
                Name = info.Name,
                Description = info.Description,
                IsActive = info.IsActive,
                ConcurrencyStamp = info.ConcurrencyStamp
            };
            return new ActionResultResponse<TitleDetailViewModel>
            {
                Code = 1,
                Data = titleDetail
            };
        }

        public async Task<ActionResultResponse<string>> UpdateIsActive(string companyId, string id, bool isActive)
        {
            var checkExit = await _titleRepository.CheckExistsAsync(companyId, id);
            if (!checkExit)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("idTitles")));
            var result = await _titleRepository.UpdateIsActive(companyId, id, isActive);
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("TitleIsActive")));
        }
    }
}
