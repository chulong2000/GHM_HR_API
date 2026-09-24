namespace GHM.HR.API.Domain.ModelMetas
{
    public class DepartmentMeta
    {
        public string CompanyId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string ConcurrencyStamp { get; set; }
        public bool IsActive { get; set; }
        public int? AdvanceLeaveGranted { get; set; }
        public int? CompLeaveGranted { get; set; }
        public int? ExpireDays { get; set; }
    }
}
