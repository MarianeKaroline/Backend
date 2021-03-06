namespace SingleExperience.Entities.ProductEntities
{
    class ProductEntitie
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Detail { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
        public float Rating { get; set; }
    }
}
