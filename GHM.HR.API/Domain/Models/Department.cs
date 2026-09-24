namespace GHM.HR.API.Domain.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public string TenantId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdPath { get; set; }
        public string NamePath { get; set; }
        public int ChildCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdateUserId { get; set; }
        public string LastUpdateFullName { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
        public string DeleteFullName { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public int? ExpireDays { get; set; }
    }
}
