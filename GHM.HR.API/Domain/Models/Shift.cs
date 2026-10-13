namespace GHM.HR.API.Domain.Models
{
    public class Shift
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }

        public string TenantId { get; set; }
        public string Name { get; set; }    
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? CreateId { get; set; }
        public string? CreateFullName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string? LastUpdateId { get; set; }
        public string? LastUpdateFullName { get; set; }
    }
}
