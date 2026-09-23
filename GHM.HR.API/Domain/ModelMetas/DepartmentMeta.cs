namespace GHM.HR.API.Domain.ModelMetas
{
    public class DepartmentMeta
    {
        public string CompanyId { get; set; } = null!;
        public int? ParentId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public int? ExpireDays { get; set; }
    }
}
