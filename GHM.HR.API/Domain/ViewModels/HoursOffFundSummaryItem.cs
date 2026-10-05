namespace GHM.HR.API.Domain.ViewModels
{
    public class HoursOffFundSummaryItem
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string DepartmentInfo { get; set; }
        public int TonDau { get; set; }   // Tồn đầu
        public int Them { get; set; }     // Thêm trong tháng
        public int HetHan { get; set; }   // Hết hạn (hiển thị màu vàng)
        public int SuDung { get; set; }   // Sử dụng
        public int TonCuoi { get; set; }  // Tồn cuối
    }

    public class HoursOffFundExpiringSoon
    {
        public int Minutes { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateLabel { get; set; }
        public int DaysLeft { get; set; }
    }
}
