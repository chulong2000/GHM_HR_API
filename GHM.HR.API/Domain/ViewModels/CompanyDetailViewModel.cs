using GHM.Infrastructure.Models;
using System.Collections.Generic;

namespace GHM.HR.Domain.ViewModels
{
    public class CompanyDetailViewModel
    {
        public List<PositionSearchViewModel> ListPositions { get; set; }
        public List<TreeData> ListDepartments { get; set; }
    }
}
