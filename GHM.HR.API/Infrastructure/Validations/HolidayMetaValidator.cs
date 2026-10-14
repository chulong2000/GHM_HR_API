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
            
            RuleFor(x => x.ToDate)
                .GreaterThan(x => x.FromDate)
                .WithMessage(ghmHRResource.GetString(ErrorMessage.Invalid, ghmHRResource.GetString("ToDate"), ghmHRResource.GetString("FromDate")));
        }
    }
}
