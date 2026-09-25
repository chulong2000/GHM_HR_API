namespace GHM.HR.Domain.Constants
{
    /// <summary>
    /// Mã công ty đặc thù cần đồng bộ mã chấm công (MaCC) với hệ thống HIS (WiseEye).
    /// </summary>
    public static class CompanyCode
    {
        public const string ThaiThinh = "TT";
        public const string Neomedic = "NEO";
    }

    /// <summary>
    /// Giới hạn nghiệp vụ khi cấp phép nghỉ cho nhân sự.
    /// </summary>
    public static class UserLeavePolicy
    {
        public const int MaxAdvanceLeaveDays = 12;
        public const int MaxCompLeaveMinutes = 480;
    }

    /// <summary>
    /// Các action gửi kèm message Kafka cho người tiêu thụ phân biệt loại thay đổi.
    /// </summary>
    public static class KafkaAction
    {
        public const string Created = "CREATED";
        public const string Edited = "EDITED";
        public const string Deleted = "DELETED";
        public const string Assigned = "ASSIGNED";
        public const string Unassigned = "UNASSIGNED";
    }
}
