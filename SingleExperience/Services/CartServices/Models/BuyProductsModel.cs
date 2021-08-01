using SingleExperience.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.CartServices.Models
{
    class BuyProductsModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public StatusProductEnum Status { get; set; }
    }
}
