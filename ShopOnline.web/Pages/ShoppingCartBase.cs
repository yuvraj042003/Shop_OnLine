using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.web.Services;
using ShopOnline.web.Services.Contracts;

namespace ShopOnline.web.Pages
{
	public class ShoppingCartBase:ComponentBase
	{
		[Inject]
		public IJSRuntime Js { get; set; }
		[Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
		public string? ErrorMessage { get; set; }

		protected string? TotalPrice { get; set; }
		protected int TotalQuantity {  get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanges();
            }
			catch (Exception ex)
			{

				ErrorMessage = ex.Message;
			}
		}
		protected async Task DeleteCartItem_Click(int id)
		{
			var cartItemDto = await ShoppingCartService.DeleteItem(id);

			RemoveCartItem(id);
            CartChanges();

		}
		private CartItemDto? GetCartItem(int id)
		{
			return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
		}
		private void SetTotalPrice()
		{
			TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
		}
		private void UpdateItemTotalPrice(CartItemDto cartItemDto) 
		{
			var items = GetCartItem(cartItemDto.Id);
			if(items != null)
			{
				items.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
			}
		}
		private void CalculateCartItemSummaryTotal()
		{
			SetTotalPrice();
			SetTotalQuantity();
		}
		private void SetTotalQuantity()
		{
			TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
		}
		protected async Task UpdateQtyCartItem_Click(int id, int qty)
		{
			try
			{
				if (qty > 0)
				{
					var updateItemDto = new CartItemQtyUpdateDto
					{
						CartItemId = id,
						Qty = qty
					}; 
					var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);
					UpdateItemTotalPrice(returnedUpdateItemDto);

                    CartChanges();

					await MakeUpdateQtyButtonVisible(id, false);

					
				}
				else
				{
					var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);

					if(item != null)
					{
						item.Qty = 1;
						item.TotalPrice = item.Price;
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		protected async Task UpdateQty_Input(int id)
		{
			await MakeUpdateQtyButtonVisible(id, true);
		}

		private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
		{
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);

        }

        private void RemoveCartItem(int id)
		{
			var cartItemDto = GetCartItem(id);

			ShoppingCartItems.Remove(cartItemDto);
		}
        private void CartChanges()
        {
			CalculateCartItemSummaryTotal();
			ShoppingCartService.RaiseEventOnShoopingCartChanged(TotalQuantity);
        }
    }
}
