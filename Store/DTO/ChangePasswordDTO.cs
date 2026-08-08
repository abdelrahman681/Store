using System.ComponentModel.DataAnnotations;

namespace Store.DTO
{
    public class ChangePasswordDTO
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The Password Required")]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The Password Required")]
        public string NewPassword { get; set; }
    }
}
