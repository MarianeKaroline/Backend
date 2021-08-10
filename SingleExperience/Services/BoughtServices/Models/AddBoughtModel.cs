using SingleExperience.Enums;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.BoughtServices.Models
{
    class AddBoughtModel
    {
        public SessionModel Session { get; set; }
        public List<BuyProductModel> BuyProducts { get; set; }
        public PaymentMethodEnum Payment { get; set; }
        public string CodeConfirmation { get; set; }
        public double TotalPrice { get; set; }
        public int AddressId { get; set; }
    }
}
