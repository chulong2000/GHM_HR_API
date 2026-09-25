using GHM.HR.Domain.Constants;
using System;
using System.Collections.Generic;

namespace GHM.HR.Domain.ModelMetas
{
    public class UserMeta
    {
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
        public string ManagerUserId { get; set; }
        public UserStatus Status { get; set; }
        public TypeMonth? Month { get; set; }
        public DateTime? OfficalDate { get; set; } 
        public DateTime JoinedDate { get; set; }
        public int? DepartmentId { get; set; }
        public string PositionId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ContractCode { get; set; }
        public WorkingForm WorkingForm { get; set; }
        public string InsuranceCode { get; set; }
        public InsuranceStatus InsuranceStatus { get; set; }
        public string InsuranceName { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public int? ExpireDays { get; set; }

    }
}