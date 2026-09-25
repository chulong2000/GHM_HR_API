namespace GHM.HR.Domain.Constants
{
    /// <summary>
    /// Tình trạng hôn nhân:
    /// 0: Độc thân
    /// 1: Đã kết hôn.
    /// 2: Ly thân.
    /// 3: Ly hôn.
    /// </summary>
    public enum MarriedStatus
    {
        Single,
        Married,
        Separated,
        Divorce
    }
}