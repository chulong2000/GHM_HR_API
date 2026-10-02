namespace GHM.HR.API.Domain.Models
{
    public class TitleDetailViewModel
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
