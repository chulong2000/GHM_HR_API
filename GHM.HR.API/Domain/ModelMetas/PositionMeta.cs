using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.ModelMetas
{
    public class PositionMeta
    {
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public bool IsMultiple { get; set; } = false;
        public bool IsActive { get; set; }
    }
}
