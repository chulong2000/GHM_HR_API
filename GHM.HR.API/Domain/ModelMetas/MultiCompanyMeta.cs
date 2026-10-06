namespace GHM.HR.API.Domain.ModelMetas
{
    public class MultiCompanyMeta
    {
        public Guid? Id { get; set; }

        public string UserId { get; set; }

        public string CompanyId { get; set; }

        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string PositionId { get; set; }

        public string PositionName { get; set; }    

        public string DoctorCode { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
