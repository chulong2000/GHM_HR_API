namespace GHM.HR.API.Domain.ViewModels
{
    public class DepartmentSearchViewModel
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IdPath { get; set; }
        public string NamePath { get; set; }
        public int? ChildCount { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
