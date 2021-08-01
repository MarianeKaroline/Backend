﻿using SingleExperience.Services.ProductServices.Models.CartModels;
using System;
using System.Collections.Generic;

namespace SingleExperience.Services.CartServices.Models
{
    class PreviewBoughtModel
    {
        public string FullName { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string Phone { get; set; }
        public string NumberCard { get; set; }
        public Enum Method { get; set; }
        public List<ProductsCartModel> Itens { get; set; }
    }
}
