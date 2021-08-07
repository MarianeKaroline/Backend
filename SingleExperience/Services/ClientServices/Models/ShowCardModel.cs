using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class ShowCardModel
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
    }
}
