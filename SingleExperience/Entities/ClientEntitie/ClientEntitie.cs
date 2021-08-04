using System;

namespace SingleExperience.Entities.ProductEntities.ClientEntities
{
    class ClientEntitie
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public int AddressId { get; set; }
    }
}
