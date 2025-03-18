using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(30, ErrorMessage = "Full Name can't be longer than 30 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(10, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Column(TypeName = "nvarchar(max)")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(20, ErrorMessage = "Country can't be longer than 50 characters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(20, ErrorMessage = "City can't be longer than 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't be longer than 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(10, ErrorMessage = "Role can't be longer than 50 characters.")]
        public string Role { get; set; } = "User";
    }
}
