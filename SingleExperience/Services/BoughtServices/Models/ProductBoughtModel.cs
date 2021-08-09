using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.BoughtServices.Models
{
    class ProductBoughtModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public int BoughtId { get; set; }
    }
}
