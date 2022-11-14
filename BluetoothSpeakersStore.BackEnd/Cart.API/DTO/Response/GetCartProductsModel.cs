﻿namespace Cart.API.DTO.Response
{
    public class GetCartProductsModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Comment { get; set; }
    }
}
