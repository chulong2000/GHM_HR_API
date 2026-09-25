using GHM.HR.Domain.Constants;
using System;

namespace GHM.HR.Domain.ViewModels
{
    public class UserSearchViewModel
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public UserStatus? Status { get; set; }
        public DateTime? JoinedDate { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string TitleName { get; set; }
        public string PositionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ManagerFullName { get; set; }
        public WorkingForm WorkingForm { get; set; }
        public PersonnelStatus PersonnelStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
    }

    public class UserCountByRelationshipViewModel
    {
        public int Official { get; set; }
        public int Probation { get; set; }
        public int Intern { get; set; }
        public int Freelancer { get; set; }
    }
}