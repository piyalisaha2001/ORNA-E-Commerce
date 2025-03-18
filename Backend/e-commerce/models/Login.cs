using System.ComponentModel.DataAnnotations;

namespace e_commerce.models
{
    public class Login
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
