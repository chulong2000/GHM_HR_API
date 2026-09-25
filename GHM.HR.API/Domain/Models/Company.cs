using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string TaxCode { get; set; }
        public bool IsActive { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdateUserId { get; set; }
        public string LastUpdateFullName { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
        public string DeleteFullName { get; set; }
        public string Logo { get; set; }
        public string LogoFooter { get; set; }

        /// <summary>Chuỗi JSON mảng Id App công ty được cấp quyền (lưu vào cột Companys.AppIds).</summary>
        public string AppIds { get; set; }

        public Company()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString();
            IsDelete = false;
            IsActive = true;
            CreateTime = DateTime.Now;
            LastUpdate = null;
            DeleteTime = null;
        }
    }
}
