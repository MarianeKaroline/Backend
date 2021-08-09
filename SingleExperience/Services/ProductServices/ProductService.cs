using SingleExperience.Services.ProductServices.Model;
using SingleExperience.Entities.DB;
using SingleExperience.Entities.ProductEntities;
using System.Collections.Generic;
using System.Linq;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.ProductServices.Models;

namespace SingleExperience.Services.ProductServices
{
    class ProductService
    {
        private ProductDB productDB;
        private ProductDB product;
        private List<ProductEntitie> list;

        public ProductService()
        {
            productDB = new ProductDB();
            product = new ProductDB();
            list = product.ListProducts();
        }

        //Listar Produtos Home
        public List<BestSellingModel> ListProducts()
        {
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
            var productCategory = new List<CategoryModel>();

            productCategory = list
                .Where(p => p.Available == true && p.CategoryId == categoryId)
                .Select(i => new CategoryModel()
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    CategoryId = i.CategoryId,
                    Available = i.Available
                })
                .ToList();                

            return productCategory;
        }

        //Listar Produto Selecionado
        public ProductSelectedModel SelectedProduct(int productId)
        {
            var selectedModels = new ProductSelectedModel();

            selectedModels = list
                .Where(p => p.Available == true && p.ProductId == productId)
                .Select(i => new ProductSelectedModel()
                {
                    Rating = i.Rating,
                    CategoryId = i.CategoryId,
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Amount = i.Amount,
                    Detail = i.Detail
                })
                .FirstOrDefault();                

            return selectedModels;
        }

        //Se existe o produto com o id que o usuário digitou
        public bool HasProduct(int code)
        {
            return list.Single(i => i.ProductId == code) != null;
        }

        //Diminui a quantidade do estoque quando a compra é confirmada pelo funcionário
        public bool Confirm(List<ProductBought> list)
        {
            var confirmed = false;

            list.ForEach(i =>
            {
                confirmed = productDB.EditAmount(i.ProductId, i.Amount);
            });

            return confirmed;
        }

        public List<ListProductsModel> ListAllProducts()
        {
            var productsModel = new List<ListProductsModel>();

            list
                .ToList()
                .ForEach(p =>
                {
                    var products = new ListProductsModel();
                    products.ProductId = p.ProductId;
                    products.Name = p.Name;
                    products.Price = p.Price;
                    products.Amount = p.Amount;
                    products.CategoryId = p.CategoryId;
                    products.Ranking = p.Ranking;
                    products.Available = p.Available;

                    productsModel.Add(products);
                });

            return productsModel;
        }
    }
}
