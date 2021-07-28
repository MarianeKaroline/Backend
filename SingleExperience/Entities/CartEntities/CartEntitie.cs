using System;

namespace SingleExperience.Entities.CartEntities
{
    class CartEntitie
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int ProductStatus { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
