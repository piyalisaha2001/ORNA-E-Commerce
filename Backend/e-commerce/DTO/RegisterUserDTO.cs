using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTO
{
    public class RegisterUserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        //public string Role { get; set; }

    }
}
