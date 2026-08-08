using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public class LoginDTO
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "The Email Required")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The Password Required")]
        public string Password { get; set; }
    }
}
