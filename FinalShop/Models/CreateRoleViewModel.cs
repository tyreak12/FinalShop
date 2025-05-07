
using System.ComponentModel.DataAnnotations;

namespace FinalShop.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = default!;
    }
}
