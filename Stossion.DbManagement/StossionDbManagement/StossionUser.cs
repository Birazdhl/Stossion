using Microsoft.AspNetCore.Identity;
using Stossion.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stossion.DbManagement.StossionDbManagement
{
    public class StossionUser : IdentityUser
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [ForeignKey("Student")]
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? VerifyAt { get; set; }

		public virtual Country Country { get; set; }
		public virtual Gender Gender { get; set; }
	}
}
