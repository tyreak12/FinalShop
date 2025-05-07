using FinalShop.Models;
namespace FinalShop.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(string? searchString = null);
        Product? GetById(int id);
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
