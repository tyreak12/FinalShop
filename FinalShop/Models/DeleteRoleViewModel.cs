namespace FinalShop.Models
{
    public class DeleteRoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new();
    }
}
