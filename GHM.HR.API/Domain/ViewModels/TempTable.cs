using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ViewModels
{
    public class TempTable
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public string PositionId { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public string Type { get; set; }
    }

    public class ShiftOfUser
    {
        public DateTime Date { get; set; }
        public int PlannedMinutes { get; set; }
    }
}
