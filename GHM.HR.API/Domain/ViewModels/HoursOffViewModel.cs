namespace GHM.HR.API.Domain.ViewModels
{
    public class HoursOffViewModel
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public int Total { get; set; }
        public string CanhBao { get; set; }
    }

    public class HoursOffDetailViewModel
    {
        public DateTime CreateTime { get; set; }
        public string TypeRequest { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Minutes { get; set; }
    }

    public class HoursOffFundDetailItem
    {
        public int Kind { get; set; }
        // 1 = OT (Thêm giờ NB)
        // 2 = NB (Nghỉ bù)
        public string RequestCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        public int ActualMinute { get; set; }
        // Thời lượng (phút)
        // Chỉ có với Kind=1 (OT)
        public string ExpiryDateLabel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActualConfirmed { get; set; }
        public string FundId { get; set; }
        public byte? Status { get; set; }
        public bool IsRefunded { get; set; }
        // FundType=1: 1=Active,2=FullyUsed,3=Expired,4=Cancelled
    }

    public class HoursOffFundExpiredDetailItem
    {
        public byte SourceType { get; set; }
        // 1 = Đơn | 2 = Ca
        public string NgayPhatSinh { get; set; }
        public int SoPhutGoc { get; set; }
        public int DaDung { get; set; }
        public int HetHan { get; set; }
        public string NgayHetHan { get; set; }
        public int TongHetHan { get; set; }
    }
}
