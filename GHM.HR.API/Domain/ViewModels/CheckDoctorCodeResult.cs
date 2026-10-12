namespace GHM.HR.API.Domain.ViewModels
{
    public sealed record CheckDoctorCodeResult(
    int DuplicateCount,
    IReadOnlyList<DoctorCodeDuplicateDto> Duplicates)
    {
        public bool HasDuplicate => DuplicateCount > 0;
    }
}
