﻿using ShopOnline.Models.Dtos;

namespace ShopOnline.web.Services.Contracts
{
	public interface IShoppingCartService
	{
		Task<List<CartItemDto>> GetItems(int userId);
		Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
		Task<CartItemDto> DeleteItem(int id);
		Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

        event Action<int> OnShoppingCartChanged;

        void RaiseEventOnShoopingCartChanged(int totalQty);
    }
}
