using GHM.HR.Domain.Constants;
using System;
namespace GHM.HR.Domain.Models
{
	public class UserOtherInfo
	{
		public string Id { get; set; }
		public string TenantId { get; set; }
		public string Address { get; set; }
		public string PermanentAddress { get; set; }
		public string TemporaryAddress { get; set; }
		public MarriedStatus? MarriedStatus { get; set; }
		public string PhoneNumber { get; set; }
        public string EnrollNumberTT { get; set; }
        public string EnrollNumberNeo { get; set; }
        public string TaxCode { get; set; }
	}
}