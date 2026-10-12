namespace GHM.HR.API.Domain.ViewModels
{
    public class BulkInsertScheduleViewModel
    {
        public int InsertedCountSuccess { get; init; }
        public IReadOnlyList<ScheduleConflict> Conflicts { get; init; } = [];
    }

    public class ScheduleConflict
    {
        public string UserId { get; init; } = string.Empty;
        public DateTime WorkDay { get; init; }
        public Guid ShiftId { get; init; }
        public string ConflictReason { get; init; } = string.Empty;
    }
}
