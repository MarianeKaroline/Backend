using System;

namespace SingleExperience.Entities.ClientEntities
{
    class CardEntitie
    {
        public int CardId { get; set; }
        public long CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime ShelfLife { get; set; }
        public int CVV { get; set; }
        public string Cpf { get; set; }
    }
}
