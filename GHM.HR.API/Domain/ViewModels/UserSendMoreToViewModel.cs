namespace GHM.HR.API.Domain.ViewModels
{
    public class UserSendMoreToViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
    }

    public class UserWorkScheduleViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsHasShiftBreak { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan? BreakDurationFrom { get; set; }
        public TimeSpan? BreakDurationTo { get; set; }
        public bool IsOvernight { get; set; }
        public int Type { get; set; }
        public int Period { get; set; }
    }
}
