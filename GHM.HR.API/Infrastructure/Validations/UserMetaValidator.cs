using FluentValidation;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.Helpers.Validations;
using GHM.Infrastructure.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.API.Domain.Resources;

namespace GHM.HR.Infrastructure.Validations
{
    public class UserMetaValidator : AbstractValidator<UserMeta>
    {
        public UserMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
        {
            RuleFor(x => x.Code)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Code")));

            RuleFor(x => x.FullName)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("FullName")));

            RuleFor(x => x.Gender).IsInEnum().WithMessage(ghmHRResource.GetString(ErrorMessage.ErrorEnum, ghmHRResource.GetString("Gender")))
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Gender")));

            RuleFor(x => x.Month).IsInEnum().WithMessage(ghmHRResource.GetString(ErrorMessage.ErrorEnum, ghmHRResource.GetString("Month")))
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Month")));

            RuleFor(x => x.JoinedDate)
                .MustBeValidDate(ghmHRResource.GetString(ErrorMessage.Invalid, ghmHRResource.GetString("JoinedDate")))
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("JoinedDate")));

            //RuleFor(x => x.OfficalDate)
            //    .MustBeValidDate(ghmHRResource.GetString(ErrorMessage.Invalid, ghmHRResource.GetString("OfficalDate")))
            //    .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("OfficalDate")));

            RuleFor(x => x.PositionId)
               .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.Invalid, ghmHRResource.GetString("Position")));
            
            RuleFor(x => x.Status)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Status")));

            RuleFor(x => x.WorkingForm)
              .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("WorkingForm")));

            RuleFor(x => x.InsuranceStatus)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("InsuranceStatus")));

            RuleFor(x => x.IsActive)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("IsActive")));

            RuleFor(x => x.CompanyId)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Company"))); 

            RuleFor(x=>x.ContractCode)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("ContractCode")));
            RuleFor(x => x.PersonnelStatus)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("PersonnelStatus")));
        }
    }
}
