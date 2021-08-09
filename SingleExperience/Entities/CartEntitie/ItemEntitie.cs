namespace SingleExperience.Entities.CartEntities
{
    class ItemEntitie
    {
        public int ItemCartId { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }     
        public int Amount { get; set; }
        public int StatusId { get; set; } 
    }
}
