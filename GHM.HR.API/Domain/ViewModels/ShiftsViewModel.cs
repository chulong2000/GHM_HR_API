namespace GHM.HR.API
{
    public class ShiftsViewModel
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}