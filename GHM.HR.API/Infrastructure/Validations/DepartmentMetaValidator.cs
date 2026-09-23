using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Resources;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using FluentValidation;

namespace GHM.HR.API.Infrastructure.Validations
{
    public class DepartmentMetaValidator : AbstractValidator<DepartmentMeta>
    {
        public DepartmentMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
        {
            RuleFor(x => x.CompanyId)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Company")));

            RuleFor(x => x.Name)
               .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Name")));
        }
    }
}
