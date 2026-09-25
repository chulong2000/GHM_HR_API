using GHM.HR.Domain.Constants;
using System;
namespace GHM.HR.Domain.Models
{
	public class UserPaperInfo
	{
		public string Id { get; set; }
		public string TenantId { get; set; }
		public string CCHNNumber { get; set; }
		public DateTime? CCHNDateOfIssue { get; set; }
		public string CCHNPlaceOfIssue { get; set; }
		public IdentityCardType TypeCard { get; set; }
		public string IdCardNumber { get; set; }
		public DateTime? IdCardDateOfIssue { get; set; }
		public string IdCardPlaceOfIssue { get; set; }
	}
}