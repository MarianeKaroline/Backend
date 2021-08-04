using SingleExperience.Services.ProductServices.Model;
using SingleExperience.Entities.ProductEntities.DB;
using SingleExperience.Entities.ProductEntities;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.ProductServices
{
    class ProductService
    {
        private ProductDB product;
        private List<ProductEntitie> list;

        public ProductService()
        {
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
            var has = false;

            if (list.Single(i => i.ProductId == code) != null)
            {
                has = true;
            }
            return has;
        }
    }
}
