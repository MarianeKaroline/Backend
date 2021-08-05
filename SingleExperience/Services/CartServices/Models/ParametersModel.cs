using SingleExperience.Entities.CartEntities;
using System.Collections.Generic;

namespace SingleExperience.Services.CartServices.Models
{
    class ParametersModel
    {
        public int CountProduct { get; set; }
        public string Session { get; set; }
        public List<ItemEntitie> CartMemory { get; set; }
    }
}
