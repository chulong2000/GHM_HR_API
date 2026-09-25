using GHM.HR.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ViewModels
{
    public class UserBirthdayViewModel
    {
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string JoinedDate { get; set; }
    }

    public class UserResignedViewModel
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime OutDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ManagerFullName { get; set; }
    }
}
