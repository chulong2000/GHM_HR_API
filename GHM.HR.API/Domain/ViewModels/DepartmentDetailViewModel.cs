namespace GHM.HR.API.Domain.ViewModels
{
    public class DepartmentDetailViewModel
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdPath { get; set; }
        public string NamePath { get; set; }
        public int? ChildCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool? IsActive { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public int? ExpireDays { get; set; }
    }

    public class DepartmentInWorkScheduleViewModel
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
