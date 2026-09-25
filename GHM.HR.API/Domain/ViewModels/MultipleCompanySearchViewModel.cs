using GHM.HR.Domain.Constants;
using System;
namespace GHM.HR.Domain.ViewModels
{
    public class MultipleCompanySearchViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public string DoctorCode { get; set; }
        public DateTime CreateTime { get; set; }
    }
}