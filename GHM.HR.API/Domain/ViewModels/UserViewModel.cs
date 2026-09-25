using GHM.HR.Domain.Constants;
using System;
namespace GHM.HR.Domain.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentIdPath { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string UserName { get; set; }
        public bool IsLock { get; set; }
        public bool IsAdmin { get; set; }
        public string ReferenceId { get; set; }
        public string EnrollNumberTT { get; set; }
        public string EnrollNumberNeo { get; set; }
    }
}