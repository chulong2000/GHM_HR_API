namespace GHM.HR.API.Domain.ViewModels
{
    public sealed record DoctorCodeDuplicateDto
    {
        public string DoctorCode { get; init; } = string.Empty;
        public int SoLanTrongDanhSach { get; init; }
        public bool DuplicatedInList { get; init; }
        public bool ExistsInDatabase { get; init; }
    }
}
