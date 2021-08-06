using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class CardModel
    {
        public long CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
        public int CVV { get; set; }
        public string Cpf { get; set; }
    }
}
