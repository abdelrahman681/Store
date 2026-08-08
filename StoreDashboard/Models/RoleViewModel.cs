using System.ComponentModel.DataAnnotations;

namespace DashBoard.Models
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Name IS Required")]
        [StringLength(256)]
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsSelected { get; set; }
    }
}
