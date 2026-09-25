using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ModelMetas
{
    public class CompanyMeta
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string TaxCode { get; set; }
        public bool IsActive { get; set; }
        //public string ConcurrencyStamp { get; set; }
        public string Logo { get; set; }
        public string LogoFooter { get; set; }

        /// <summary>
        /// Danh sách Id các App công ty được cấp quyền truy cập (dropdown chọn App).
        /// Bỏ trống (null) khi cập nhật → giữ nguyên danh sách hiện tại; gửi mảng rỗng → xóa hết.
        /// </summary>
        //public List<string> AppIds { get; set; }
    }
}
