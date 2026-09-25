// ----------------------------------------------o0o----------------------------------------------
// Copyright (c) Ghmsoft 2025. All rights reserved.
// Licensed under the Ghmsoft.vn License, Version 1.0. See LICENSE in the project root for license information.
// Create by : TruongTV
// Create date : 16/01/2025 15:01:53
// Description :
// Output :
// Modify :
// Project : HR
// ----------------------------------------------o0o----------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.ViewModels;

namespace GHM.HR.Domain.IRepository
{
	public interface ICompanyRepository
	{
		Task<List<CompanysSearchViewModel>> SelectAllAsync(string tenantId);
        Task<List<CompanyUserViewModel>> SelectAllByUserAsync(string tenantId, string userId);
        Task<int> InsertAsync(Company company);
		Task<int> UpdateAsync(Company company);
		Task<int> DeleteAsync(Company company);
		Task<int> ForceDeleteAsync(string id);
		Task<Company> GetInfoAsync(string id);
		Task<Company> GetInfoAsync(string tenantId, string id);
		Task<Company> GetInfoByCodeAsync(string tenantId, string code);
		Task<bool> CheckExistsAsync(string id);
		Task<bool> CheckExistsByTenantIdAsync(string tenantId, string id);
		Task<bool> CheckExistsNameAsync(string tenantId, string id,string name);
		Task<bool> CheckExistsCodeAsync(string tenantId, string id,string code);
		Task<bool> CheckExistUserAsync(string tenantId, string companyId);
		Task<string> GetCodeCompanyAsync(string companyId);
		Task<CompanyLogoViewModel> GetLogoAsync(string tenantId, string companyId);
        Task<string> GetConnectionAsync(string companyId);
    }
}