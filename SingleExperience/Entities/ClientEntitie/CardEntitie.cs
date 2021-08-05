using System;

namespace SingleExperience.Entities.ClientEntities
{
    class CardEntitie
    {
        public long CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
        public int CVV { get; set; }
        public string UserId { get; set; }
    }
}
