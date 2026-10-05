namespace GHM.HR.API.Domain.Models
{
    public class MultiCompany
    {
       public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public Guid CompanyId { get; set; } 
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
        public string DoctorCode { get; set; }
        public DateTime CreateTime { get; set; }

        public Guid CreatorId { get; set; }

        public string CreatorFullName { get; set; }
    }
}
