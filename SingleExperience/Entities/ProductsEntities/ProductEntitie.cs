using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities
{
    class ProductEntitie
    {
        public int ProdutoID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Details { get; set; }
        public int StatusId { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int Ranking { get; set; }
        public bool Available { get; set; }
        public float Rating { get; set; }

        public ProductEntitie()
        {

        }

        public ProductEntitie(int produtoID, string name, double price, string details, int statusId, int amount, int categoryId, int ranking, bool available, float rating)
        {
            ProdutoID = produtoID;
            Name = name;
            Price = price;
            Details = details;
            StatusId = statusId;
            Amount = amount;
            CategoryId = categoryId;
            Ranking = ranking;
            Available = available;
            Rating = rating;
        }

        public ProductEntitie(int produtoID, string name, double price, int statusId, int amount, int categoryId, int ranking, bool available, float rating)
        {
            ProdutoID = produtoID;
            Name = name;
            Price = price;
            StatusId = statusId;
            Amount = amount;
            CategoryId = categoryId;
            Ranking = ranking;
            Available = available;
            Rating = rating;
        }
    }
}
