using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.CartServices.Models
{
    class TotalCartModel
    {
        public int CartId { get; set; }
        public double TotalPrice { get; set; }
        public int TotalAmount { get; set; }
    }
}
