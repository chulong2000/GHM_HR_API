namespace GHM.HR.API.Domain.ViewModels
{
    public class DepartmentDetailViewModel
    {
        public int Id { get; set; }
        public string CompanyId { get; set; } = null!;
        public int? ParentId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ChildCount { get; set; }
        public bool? IsActive { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public int? ExpireDays { get; set; }
    }
}
