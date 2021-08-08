using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ProductServices.Models
{
    class AddNewProductModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Detail { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
        public float Rating { get; set; }
    }
}
