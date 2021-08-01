using System;
using SingleExperience.Entities;
using System.IO;
using System.Collections.Generic;
using SingleExperience.Services.ProductServices.Model;
using System.Text;
using System.Linq;
using SingleExperience.Services.ProductServices.Models.ProductModels;
using System.Reflection;
using SingleExperience.Entities.Enums;
using SingleExperience.Entities.DB;

namespace SingleExperience.Services.ProductServices
{
    class ProductService
    {
        //Listar Produtos Home
        public List<BestSellingModel> ListProductsTable()
        {
            ProductDB product = new ProductDB();
            var list = product.ListProducts();
            var bestSellingModel = new List<BestSellingModel>();

            list
                .Where(p => p.Available == true && p.Ranking > 0)
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
            ProductDB product = new ProductDB();
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
        public ProductSelectedModel ListProductSelected(int productId)
        {
            ProductDB product = new ProductDB();
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
    }
}
