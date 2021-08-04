namespace SingleExperience.Services.CartServices.Models
{
    class CartModel
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }
    }
}
