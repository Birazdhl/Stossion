using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.ViewModels.User
{
    public class RegisterViewModel
    {
		private string _phoneNumber = string.Empty;
		private string _lastname = string.Empty;

		[Required(ErrorMessage = "UserName is required.")]
		[RegularExpression(@"^[a-zA-Z0-9_!@#$%^&*()-+=]{5,20}$", ErrorMessage = "Username must be greater 4 and less than 21")]

		public string UserName { get; set; } = string.Empty;


		[Required(ErrorMessage = "UserName is required.")]
        public string FirstName { get; set; } = string.Empty;

		public string LastName {
			get { return _lastname ?? string.Empty; }
			set { _lastname = value; }
		} 
        public string Country { get; set; } = string.Empty;


        [Required]
        [DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()])[A-Za-z\d@$!%*?&#^()]{8,}$")]
		public string Password { get; set; } = string.Empty;


		[Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

		public string PhoneNumber {
			get { return _phoneNumber ?? string.Empty; }
			set { _phoneNumber = value; }
		}

		[Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$" , ErrorMessage = "Please enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;


		[Required]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }


		[Range(1, 3, ErrorMessage = "Gender must be Male, Female, or Not Specified.")]
		public int Gender { get; set; } 
    }
}
