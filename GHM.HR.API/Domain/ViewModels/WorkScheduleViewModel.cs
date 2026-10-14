namespace GHM.HR.API
{
    public class WorkScheduleViewModel
    {
        public DateTime WorkDay { get; set; }

        public string HolidayName { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int month { get; set; }

        public int Year { get; set; }   
    }
}