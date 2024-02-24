using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
    public class ProductBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable <ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
        }
        
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
				   group product by product.CategoryId into prodByCatGroup
				   orderby prodByCatGroup.Key
				   select prodByCatGroup;
		}
        protected string GetCategoryName(IGrouping<int, ProductDto> groupProductDtos)
        {
            return groupProductDtos.FirstOrDefault(pg => pg.CategoryId == groupProductDtos.Key).CategoryName;
        } 

    }
}
