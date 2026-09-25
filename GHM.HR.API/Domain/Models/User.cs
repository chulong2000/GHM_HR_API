using GHM.HR.Domain.Constants;
using System;
namespace GHM.HR.Domain.Models
{
	public class User
	{
        public string Id { get; set; }
        public string Code { get; set; }
        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
        public string UserName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string NationId { get; set; }
        public string NationName { get; set; }
        public string ReligionId { get; set; }
        public string ReligionName { get; set; }
        public MarriedStatus MarriedStatus { get; set; }
        public UserStatus Status { get; set; }
        public TypeMonth? Month { get; set; }
        public DateTime? OfficalDate { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentPath { get; set; }
        public string DepartmentName { get; set; }
        public string TitleId { get; set; }
        public string TitleName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ManagerUserId { get; set; }
        public string ManagerFullName { get; set; }
        public string SuccessorUserId { get; set; }
        public string SuccessorFullName { get; set; }
        public WorkingForm WorkingForm { get; set; }
        public string TaxCode { get; set; }
        public string EnrollNumberTT { get; set; }
        public string EnrollNumberNeo { get; set; }
        public string Note { get; set; }
        public string ContractCode { get; set; }
        public string InsuranceCode { get; set; }
        public InsuranceStatus InsuranceStatus { get; set; }
        public string InsuranceName { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime? IdCardDateOfIssue { get; set; }
        public string IdCardPlaceOfIssue { get; set; }
        public bool IsActive { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdateUserId { get; set; }
        public string LastUpdateFullName { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
        public string DeleteFullName { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public string Password { get; set; }    
        public DateTime? ContractExpirationDate { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public int? ExpireDays { get; set; }
        public User()
		{
			Birthday = null;
			Avatar = string.Empty;
			MarriedStatus = 0;
			Status = UserStatus.Probation;
			IsActive = true;
			CreateTime = DateTime.Now;
			ConcurrencyStamp = Guid.NewGuid().ToString();
			LastUpdate = null;
			IsDelete = false;
			DeleteTime = null;
		}
	}
}