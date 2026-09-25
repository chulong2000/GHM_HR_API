// ----------------------------------------------o0o----------------------------------------------
// Copyright (c) Ghmsoft 2025. All rights reserved.
// Licensed under the Ghmsoft.vn License, Version 1.0. See LICENSE in the project root for license information.
// Create by : TruongTV
// Create date : 16/01/2025 15:01:24
// Description :
// Output :
// Modify :
// Project : Core
// ----------------------------------------------o0o----------------------------------------------

using System;
using System.Collections.Generic;
namespace GHM.HR.Domain.ViewModels
{
	public class CompanysDetailViewModel
	{ 
		public string Id { get; set; } 
		public string Code { get; set; } 
		public string Name { get; set; } 
		public string PhoneNumber { get; set; } 
		public string Address { get; set; } 
		public string Description { get; set; } 
		public string TaxCode { get; set; } 
		public bool IsActive { get; set; } 
		public string ConcurrencyStamp { get; set; }
		public string Logo { get; set; }
		public string LogoFooter { get; set; }

		/// <summary>Danh sách Id App công ty được cấp quyền (giải mã từ Companys.AppIds) — để prefill form.</summary>
		//public List<string> AppIds { get; set; }
    }
}