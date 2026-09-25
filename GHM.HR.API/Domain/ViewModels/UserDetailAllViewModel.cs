using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ViewModels
{
    public class UserDetailAllViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
