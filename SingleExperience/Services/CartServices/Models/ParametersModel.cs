using SingleExperience.Entities.ProductEntities.CartEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.CartServices.Models
{
    class ParametersModel
    {
        public int CountProduct { get; set; }
        public string Session { get; set; }
        public List<ItemEntitie> CartMemory { get; set; }
    }
}
