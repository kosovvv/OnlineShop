﻿using OnlineShop.Data.Common.Models;
using OnlineShop.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class Product : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        [NotMapped]
        public double AverageScore => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Score) : 0;

    }

}
