using FluentValidation;
using GHM.HR.API.Domain.Resources;
using GHM.HR.Domain.ModelMetas;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.Helpers.Validations;
using GHM.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Infrastructure.Validations
{
    public class PositionMetaValidator: AbstractValidator<PositionMeta>
    {
        public PositionMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
        {
            RuleFor(x => x.CompanyId)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Company")));

            RuleFor(x => x.Code)
                .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Code")));

            RuleFor(x => x.Name)
               .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Name")));


            RuleFor(x => x.IsActive)
               .NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("IsActive")));
        }
    }
}
