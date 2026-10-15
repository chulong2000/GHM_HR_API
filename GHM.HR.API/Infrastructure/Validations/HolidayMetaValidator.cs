using FluentValidation;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Resources;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;

namespace GHM.HR.API.Infrastructure.Validations
{
    public class HolidayMetaValidator : AbstractValidator<HolidayMeta>
    {
        public HolidayMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
        {
            
            RuleFor(x => x.ToDay)
                .GreaterThan(x => x.FromDay)
                .WithMessage(ghmHRResource.GetString(ErrorMessage.Invalid, ghmHRResource.GetString("ToDay"), ghmHRResource.GetString("FromDay")));
        }
    }
}
