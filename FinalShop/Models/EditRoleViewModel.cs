namespace FinalShop.Models
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<UserRoleViewModel> Users { get; set; } = new();
    }

}
