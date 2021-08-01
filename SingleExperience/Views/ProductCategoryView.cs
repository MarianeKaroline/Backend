﻿using SingleExperience.Services.ProductServices;
using SingleExperience.Views;
using SingleExperience.Entities.Enums;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SingleExperience.Services.ProductServices.Models.ProductModels;
using SingleExperience.Services.ClientServices;

namespace SingleExperience.Views
{
    class ProductCategoryView
    {
        ProductService product = new ProductService();
        //Chama ListaProdutos pela Categoria
        public void Category(int id, int countProductCart, string session)
        {
            Console.Clear();
            var category = (CategoryProductEnum)id;
            Console.WriteLine($"\nInício > Pesquisa > {category}\n");

            ListProducts(id);
            Menu(countProductCart, session);
        }
        
        //Menu dos Produtos
        public void Menu(int countProductCart, string session)
        {
            var client = new ClientService();
            var selectedProduct = new SelectedProductView();
            var signIn = new SignInView();
            var signUp = new SignUpView();
            var cart = new CartView();
            var inicio = new HomeView();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            if (session.Length == 10)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema");
            Console.WriteLine("\n5. Selecionar um produto");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts(countProductCart, session);
                    break;
                case 1:
                    inicio.Search(countProductCart, session);
                    break;
                case 2:
                    cart.ListCart(session);
                    break;
                case 3:
                    if (session.Length == 11)
                    {
                        client.SignOut();
                        ListProducts(op);
                    }
                    else
                    {
                        signIn.Login(countProductCart, session, true);
                    }
                    break;
                case 4:
                    signUp.SignUp(countProductCart, true);
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: ");
                    int code = int.Parse(Console.ReadLine()); 
                    if (product.HasProduct(code))
                    {
                        selectedProduct.SelectedProduct(code, countProductCart, session);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(countProductCart, session);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(countProductCart, session);
                    break;
            }

            
        }
        //Listar Produtos selecionado
        public void ListProducts(int categoryId)
        {
            var productService = new ProductService();
            var itemCategory = productService.ListProductCategory(categoryId)
                .GroupBy(i => i.Name)
                .Select(j => new CategoryModel()
                {
                    ProductId = j.First().ProductId,
                    Name = j.First().Name,
                    Price = j.First().Price,
                    CategoryId = j.First().CategoryId,
                    Available = j.First().Available
                })
                .ToList();
            var j = 41;

            itemCategory.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"|R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
        }
    }
}
