using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.Resources;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.HR.Infrastructure.Repository;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Infrastructure.Service
{
    public class ShiftService : IShiftService
    {

        private readonly IShiftRepository _shiftRepository;
        private readonly ICompanyRepository _companyRepository;


        private readonly IResourceService<GhmHRResource> _ghmHRResource;

        public ShiftService(IShiftRepository shiftRepository,
                            ICompanyRepository companyRepository,
                              IResourceService<GhmHRResource> ghmHRResource)
        {
            _shiftRepository = shiftRepository;
            _companyRepository = companyRepository;
            _ghmHRResource = ghmHRResource;
        }

        public async Task<ActionResultResponse> DeleteAsync(string id)
        {
            var info = await _shiftRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Shift")));

            var result = await _shiftRepository.DeleteAsync(info);

            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Shift")));
        }

        public async Task<ActionResultResponse<ShiftsViewModel>> GetDetailAsync(string id)
        {
            var info = await _shiftRepository.GetInfoAsync(id);

            var companyName = await _companyRepository.GetInfoAsync(info.CompanyId);

            var result = new ShiftsViewModel
            {
                CompanyId = info.CompanyId,
                CompanyName = companyName.Name,
                ShiftName = info.Name,
                StartTime = info.StartTime,
                EndTime = info.EndTime
            };

            return new ActionResultResponse<ShiftsViewModel>(1, _ghmHRResource.GetString(SuccessMessage.GetDataSuccessfully, _ghmHRResource.GetString("Shift")), string.Empty, result);

        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, ShiftMeta shiftMeta)
        {
            var shiftId  = Guid.NewGuid().ToString();

            // kiểm tra tên của ca đã tồn tại hay chưa.
            var existingShift = await _shiftRepository.checkNameExists(tenantId, companyId, shiftMeta.Name.Trim(), shiftId);
            if (existingShift)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Shift"), shiftMeta.Name));

            var shift = new Shift
            {
                Id = shiftId,
                CompanyId = companyId,
                TenantId = tenantId,
                Name = shiftMeta.Name.Trim(),
                StartTime = shiftMeta.StartTime,
                EndTime = shiftMeta.EndTime,
                CreateId = createId,
                CreateFullName = createFullName,
                CreateTime = DateTime.Now,
            };

            var result = await _shiftRepository.InsertAsync(shift);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Company")), string.Empty, companyId);
        }

        public async Task<List<ShiftsViewModel>> SelectAllByCompanyIdAsync(string tenantId, string companyId)
        {
            return await _shiftRepository.SelectAllAsync(tenantId, companyId);
        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string updateId, string updateFullName, ShiftMeta shiftMeta, string shiftId, string companyId, string tenantId)
        {
            var info = await _shiftRepository.GetInfoAsync(shiftId);
            if (info == null)
                return new ActionResultResponse<string>(-1, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Shift")));

            var existingShift = await _shiftRepository.checkNameExists(tenantId, companyId, shiftMeta.Name.Trim(), tenantId);
            if (existingShift)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Shift"), shiftMeta.Name));

            info.Name = shiftMeta.Name.Trim();
            info.StartTime = shiftMeta.StartTime;
            info.EndTime = shiftMeta.EndTime;
            info.LastUpdateId = updateId;
            info.LastUpdateFullName = updateFullName;
            info.LastUpdateTime = DateTime.Now;

            var result = await _shiftRepository.UpdateAsync(info);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Shift")), string.Empty);
        }
    }
}
