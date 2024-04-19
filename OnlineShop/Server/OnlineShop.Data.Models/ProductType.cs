﻿using OnlineShop.Data.Common.Models;

namespace OnlineShop.Models
{
    public class ProductType : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public ICollection<Product> Products { get; set;} = new List<Product>();
    }
}