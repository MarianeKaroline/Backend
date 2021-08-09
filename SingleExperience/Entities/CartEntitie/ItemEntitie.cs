namespace SingleExperience.Entities.CartEntities
{
    class ItemEntitie
    {
        public int ProductCartId { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }        
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }        
    }
}
