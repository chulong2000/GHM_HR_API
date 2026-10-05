namespace GHM.HR.API.Domain.ViewModels
{
    public class UserDayOffViewModel
    {
        public decimal Total { get; set; }
        public decimal TotalUsed { get; set; }
        public decimal TotalUnUsed { get; set; }
        public int AdvanceLeaveGranted { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDateOld { get; set; }
        public decimal WorkingHours { get; set; }
        public decimal NewUnUsed { get; set; }
        public decimal OldUnUsed { get; set; }
        public decimal NewUsed { get; set; }
        public decimal OldUsed { get; set; }
    }

    public class UserHoursOffViewModel
    {
        public int TotalMinute { get; set; }
        public int TotalMinuteUnUsed { get; set; }
        public int CompLeaveGranted { get; set; }
        public decimal WorkingHours { get; set; }
    }

    public class UserHoursOffDetailViewModel
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Total { get; set; }
        public int TotalUsed { get; set; }
        public int TotalUnUsed { get; set; }
        public int TotalClear { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
