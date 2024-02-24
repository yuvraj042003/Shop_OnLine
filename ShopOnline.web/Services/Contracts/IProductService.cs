using ShopOnline.Models.Dtos;

namespace ShopOnline.web.Services.Contracts
{
    public interface IProductService
    {
        
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);
    }
}
