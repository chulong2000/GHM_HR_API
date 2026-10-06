namespace GHM.HR.API.Domain.ModelMetas
{
    public class MultiCompanyMeta
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public Guid PositionId { get; set; }

        public string PositionName { get; set; }    

        public string DoctorCode { get; set; }

        public DateTime CreateTime { get; set; }

        public List<MultiCompanyMeta> multiCompanyMetas { get; set; }
    }
}
