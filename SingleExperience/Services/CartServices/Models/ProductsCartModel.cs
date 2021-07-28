using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Models.CartModels
{
    class ProductsCartModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public int TotalAmount { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
    }
}
