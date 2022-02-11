using DataLayer.Interfaces;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicaLayer.ProductService;
using System;

namespace TestJuniorServiceTest
{
    [TestClass]
    public class ProductServiceTest
    {
        private readonly ProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly MyContext _context;
        private Brand _brand;
        private Account _account
        public ProductServiceTest(ProductService productService, IProductRepository productRepository, MyContext context)
        {
            _productService = productService;
            _productRepository = productRepository;
            _context = context;
        }
        [TestMethod]
        public void CreateProduct_success()
        {
            Product product=new Product();
            product.Name =new Guid().ToString();
            product.Description = new Guid().ToString();
            product.Price = 10101001010101010;
            product.BrandId = _brand.Id;




        }



        [ClassInitialize]
        private void ClassInitializer()
        {
            Account account = new Account();
            account.Email =new Guid().ToString()+"@gmail.com";
            account.Password = new Guid().ToString();
            Brand brand = new Brand();
            brand.BrandName=new Guid().ToString();
            brand.Description = new Guid().ToString();

            


            _context.Add(account);
            _context.Add(brand);

            _brand = brand;
            _account = account;

        }
        [ClassCleanup]
        private void ClassCleanup()
        {


            _context.Remove(_account);
            _context.Remove(_brand);
        }
    }
}
