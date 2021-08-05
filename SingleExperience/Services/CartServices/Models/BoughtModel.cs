using SingleExperience.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.CartServices.Models
{
    class BoughtModel
    {
        public string Session { get; set; }
        public PaymentMethodEnum Method { get; set; }
        public string Confirmation { get; set; }
        public StatusProductEnum Status { get; set; }
        public List<int> Ids { get; set; }
    }
}
