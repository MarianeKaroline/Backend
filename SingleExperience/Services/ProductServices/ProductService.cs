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

namespace SingleExperience.Services.ProductServices
{
    class ProductService
    {
        //Lê o arquivo CSV Produtos
        public List<ProductEntitie> ListProducts()
        {
            var CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string path = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            var prod = new List<ProductEntitie>();

            try
            {
                string[] products = File.ReadAllLines(path, Encoding.UTF8);

                using (StreamReader sr = File.OpenText(path))
                {
                    products
                        .Skip(1)
                        .ToList()
                        .ForEach(item =>
                        {
                            string[] fields = item.Split(',');

                            var produto = new ProductEntitie();

                            produto.ProductId = int.Parse(fields[0]);
                            produto.Name = fields[1];
                            produto.Price = double.Parse(fields[2]);
                            produto.Detail = fields[3];
                            produto.Amount = int.Parse(fields[4]);
                            produto.CategoryId = int.Parse(fields[5]);
                            produto.Ranking = int.Parse(fields[6]);
                            produto.Available = bool.Parse(fields[7]);
                            produto.Rating = float.Parse(fields[8]);


                            prod.Add(produto);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prod;
        }

        //Listar Produtos Home
        public List<BestSellingModel> ListProductsTable()
        {
            var list = ListProducts();
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
            var list = ListProducts();
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
            var list = ListProducts();
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
