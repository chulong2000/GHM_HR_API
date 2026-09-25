using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Domain.Constants
{
    public enum PersonnelStatus
    {
        Working = 0, // Dang lam viec
        UnpaidLeave = 1, // Nghi khong luong
        MaternityLeave = 2, // Nghi thai san
        Resigned = 3, // Da nghi viec
    }
}
