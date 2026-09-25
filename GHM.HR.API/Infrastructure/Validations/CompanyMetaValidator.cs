// ----------------------------------------------o0o----------------------------------------------
// Copyright (c) Ghmsoft 2025. All rights reserved.
// Licensed under the Ghmsoft.vn License, Version 1.0. See LICENSE in the project root for license information.
// Create by : Nguyen Dac Quang
// Create date : 18/02/2025 10:48:21
// Description :Bang quy trinh
// Output :
// Modify :
// Project :Quản lý Nhân Sự
// ----------------------------------------------o0o----------------------------------------------

using FluentValidation;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.Helpers.Validations;
using GHM.Infrastructure.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.API.Domain.Resources;

namespace GHM.HR.Infrastructure.Validations
{
	public class CompanyMetaValidator : AbstractValidator<CompanyMeta>
	{
		public CompanyMetaValidator(IResourceService<GhmHRResource> ghmHRResource)
		{
			RuleFor(x => x.Code)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Code")));

			RuleFor(x => x.Name)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Name")));

			RuleFor(x => x.PhoneNumber)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("PhoneNumber")));

			RuleFor(x => x.Address)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("Address")));

			RuleFor(x => x.TaxCode)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("TaxCode")));

			RuleFor(x => x.IsActive)
				.NotNullAndEmpty(ghmHRResource.GetString(ErrorMessage.CanNotBeNull, ghmHRResource.GetString("IsActive")));
		}
	}
}
