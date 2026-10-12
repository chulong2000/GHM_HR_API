namespace GHM.HR.API.Domain.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public int ShiftType { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? CreateId { get; set; }
        public string? CreateFullName { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public Guid? LastUpdatId { get; set; }
        public string? LastUpdateFullName { get; set; }
    }
}
