using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ViewModels
{
    public class UserSearchAllViewModel
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
