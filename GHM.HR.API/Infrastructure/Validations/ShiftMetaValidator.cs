using FluentValidation;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Resources;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Helpers.Validations;

namespace GHM.HR.API.Infrastructure.Validations
{
    public class ShiftMetaValidator : AbstractValidator<ShiftMeta>
    {
        public ShiftMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
        {
            RuleFor(x => x.Name)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Shift")));

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage(ghmHRResource.GetString(ErrorMessage.InvalidEndTime, ghmHRResource.GetString("EndTime"), ghmHRResource.GetString("StartTime")));
        }
    }
}
