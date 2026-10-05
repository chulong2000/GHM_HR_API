namespace GHM.HR.API.Domain.ViewModels
{
    public class WorkShiftRangeResponse
    {
        // Danh sách các ca làm việc trùng khớp
        public List<ShiftOverlapDetail> Shifts { get; set; }

        // Thông tin trạng thái và tổng kết
        public WorkShiftStatus Status { get; set; }

        // Constructor khởi tạo mặc định để tránh lỗi NullReference
        public WorkShiftRangeResponse()
        {
            Shifts = new List<ShiftOverlapDetail>();
            Status = new WorkShiftStatus();
        }
    }

    public class WorkShiftStatus
    {
        // 1: Thành công, 0: Không hợp lệ, 2: Trùng ca khác
        public int Code { get; set; }

        public string Message { get; set; }

        // Tổng số phút của tất cả các ca cộng lại
        public int TotalMinute { get; set; }

        // Tính toán thêm thuộc tính giờ (Read-only) để hiển thị nếu cần
        public decimal TotalHours => Math.Round((decimal)TotalMinute / 60, 2);
    }

    public class ShiftOverlapDetail
    {
        public string ShiftId { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public int? ShiftType { get; set; }

        // Số phút giao nhau tính được
        public int Minutes { get; set; }

        // Thời gian đã được định dạng HH:mm từ SQL
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
