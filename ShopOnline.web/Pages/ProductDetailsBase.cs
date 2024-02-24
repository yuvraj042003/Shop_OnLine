using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.web.Services;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
	public class ProductDetailsBase:ComponentBase
	{
		[Parameter]
		public int Id { get; set; }
		[Inject]
		public IProductService ProductService { get; set; }
		[Inject]
		public IShoppingCartService ShoppingCartService { get; set; }
		[Inject]
		public  NavigationManager NavigationManager { get; set; }
		public ProductDto Product { get; set; }
		public string ErrorMessage { get; set; }
		protected override async Task OnInitializedAsync()
		{
			try
			{
				Product = await ProductService.GetItem(Id);
				var ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
				var totalQty = ShoppingCartItems.Sum(i => i.Qty);

				ShoppingCartService.RaiseEventOnShoopingCartChanged(totalQty);
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}
		protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
		{
			try
			{
				var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);
				NavigationManager.NavigateTo("/ShoppingCart");


			}
			catch (Exception)
			{

				throw;
			}
		}
		
	}
}
