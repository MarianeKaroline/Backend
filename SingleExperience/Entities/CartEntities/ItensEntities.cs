﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.CartEntities
{
    class ItensEntities
    {
        public int ProductCartId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }
    }
}