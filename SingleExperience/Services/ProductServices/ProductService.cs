using System.Collections.Generic;
using SingleExperience.Services.ProductServices.Model;
using System.Linq;
using SingleExperience.Services.ProductServices.Models.ProductModels;
using SingleExperience.Entities.DB;

namespace SingleExperience.Services.ProductServices
{
    class ProductService
    {
        ProductDB product;

        public ProductService()
        {
            product = new ProductDB();
        }

        //Listar Produtos Home
        public List<BestSellingModel> ListProducts()
        {
            var list = product.ListProducts();
            var bestSellingModel = new List<BestSellingModel>();

            list
                .Where(p => p.Available == true)
                .OrderByDescending(p => p.Ranking)
                .Take(5)
                .ToList()
                .ForEach(p =>
                {
                    var selling = new BestSellingModel();
                    selling.ProductId = p.ProductId;
                    selling.Name = p.Name;
                    selling.Price = p.Price;
                    selling.Ranking = p.Ranking;
                    selling.Available = p.Available;

                    bestSellingModel.Add(selling);
                });

            return bestSellingModel;
        }

        //Listar Produtos Categoria
        public List<CategoryModel> ListProductCategory(int categoryId)
        {
            var list = product.ListProducts();
            var bestSelling = new List<CategoryModel>();

            list
                .Where(p => p.Available == true && p.CategoryId == categoryId)
                .ToList()
                .ForEach(p =>
                {
                    var selling = new CategoryModel();
                    selling.ProductId = p.ProductId;
                    selling.Name = p.Name;
                    selling.Price = p.Price;
                    selling.CategoryId = p.CategoryId;
                    selling.Available = p.Available;

                    bestSelling.Add(selling);
                });

            return bestSelling;
        }

        //Listar Produto Selecionado
        public ProductSelectedModel SelectedProduct(int productId)
        {
            var list = product.ListProducts();
            var selectedModels = new ProductSelectedModel();

            list
                .Where(p => p.Available == true && p.ProductId == productId)
                .ToList()
                .ForEach(p =>
                {
                    var product = new ProductSelectedModel();
                    product.Rating = p.Rating;
                    product.CategoryId = p.CategoryId;
                    product.ProductId = p.ProductId;
                    product.Name = p.Name;
                    product.Price = p.Price;
                    product.Amount = p.Amount;
                    product.Detail = p.Detail;

                    selectedModels = product;
                });

            return selectedModels;
        }

        public bool HasProduct(int code)
        {
            var list = product.ListProducts();
            var has = false;

            if (list.Single(i => i.ProductId == code) != null)
            {
                has = true;
            }
            return has;
        }
    }
}
