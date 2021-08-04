using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Model
{
    class CategoryModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public bool Available { get; set; }
    }
}
