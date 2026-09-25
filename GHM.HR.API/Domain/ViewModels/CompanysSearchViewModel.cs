// ----------------------------------------------o0o----------------------------------------------
// Copyright (c) Ghmsoft 2025. All rights reserved.
// Licensed under the Ghmsoft.vn License, Version 1.0. See LICENSE in the project root for license information.
// Create by : TruongTV
// Create date : 16/01/2025 15:01:09
// Description :
// Output :
// Modify :
// Project : HR
// ----------------------------------------------o0o----------------------------------------------

using System;
using System.Collections.Generic;
namespace GHM.HR.Domain.ViewModels
{
	public class CompanysSearchViewModel
	{ 
		public string Id { get; set; } 
		public string Code { get; set; } 
		public string Name { get; set; } 
		public string PhoneNumber { get; set; } 
		public string Address { get; set; } 
		public string Description { get; set; } 
		public string TaxCode { get; set; } 
		public bool IsActive { get; set; } 
		//public string ConcurrencyStamp { get; set; } 
	}
}