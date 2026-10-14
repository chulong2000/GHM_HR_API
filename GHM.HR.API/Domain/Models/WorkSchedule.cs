namespace GHM.HR.API.Domain.Models
{
    public class WorkSchedule
    {
        public Guid Id { get; set; }
        public DateTime WorkDay { get; set; }
        public Guid ShiftId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? CreateId { get; set; }
        public string? CreateFullName { get; set; }
        public Guid CompanyId { get; set; }
        public Guid TenantId { get; set; }
        public int Year { get; set; }
    }
}
