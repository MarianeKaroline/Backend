using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Models
{
    class ListProductsModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
    }
}
