namespace GHM.HR.API.Domain.Models
{
    public class Holiday { 
        public Guid Id { get; set; } 
        public Guid TenantId { get; set; } 
        public Guid CompanyId { get; set; } 
        public DateTime FromDay { get; set; } 
        public DateTime ToDay { get; set; } 
        public string Name { get; set; } 
        public string? Description { get; set; } 
        public DateTime CreateTime { get; set; } 
        public Guid CreateId { get; set; } 
        public string CreateFullName { get; set; } 
        public DateTime? LastUpdateTime { get; set; } 
        public Guid? LastUpdateId { get; set; } 
        public string? LastUpdateFullName { get; set; }

        public bool IsDelete { get; set; }
    }
}
