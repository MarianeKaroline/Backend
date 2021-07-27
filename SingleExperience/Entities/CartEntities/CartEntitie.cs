using System;

namespace SingleExperience.Entities.CartEntities
{
    class CartEntitie
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateCreated { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
