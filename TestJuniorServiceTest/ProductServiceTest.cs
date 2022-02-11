using DataLayer.Interfaces;
using Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicaLayer.ProductService;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestJuniorEFAPI;

namespace TestJuniorServiceTest
{
    [TestClass]
    public class ProductServiceTest
    {
        private DependencyResolverHelper _serviceProvider;
        private MyContext _context;
        private ProductService _productService;
        private IProductRepository _productRepository;

        private Brand brand;
        private Account account;
        private Category category1;
        private Category category2;
        private Category category3;


        public ProductServiceTest()
        {

            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new DependencyResolverHelper(webHost);
        }


        [TestMethod]
        public async Task CreateProduct()
        {

            Product product=new Product();
            product.Name =Guid.NewGuid().ToString();
            product.Description = Guid.NewGuid().ToString();
            product.Price = 101010010;
            product.BrandId = brand.Id;
            product.ShortDescription = Guid.NewGuid().ToString();
            int[] catIds =new int[3];
            catIds[0] = category1.Id;
            catIds[1] = category2.Id;
            catIds[2] = category3.Id;


             await _productService.AddProduct(product,catIds);


            var prodFromRepo= _productRepository.GetById(product.Id).Include(x=>x.ProductCategories).Include(x=>x.Brand).FirstOrDefault();


            Assert.IsNotNull(prodFromRepo);
            Assert.AreEqual(product.Id,prodFromRepo.Id);
            Assert.AreEqual(product.Name,prodFromRepo.Name);
            Assert.AreEqual(product.Description,prodFromRepo.Description);
            Assert.AreEqual(product.Price,prodFromRepo.Price);
            Assert.AreEqual(product.BrandId,prodFromRepo.BrandId);
            Assert.IsTrue(prodFromRepo.ProductCategories.ToList().Select(x=>x.CategoryId).Contains(category1.Id));

        }



        [TestInitialize]
        public  void TestInitializer()
        {
            _context = _serviceProvider.GetService<MyContext>();
            _productRepository = _serviceProvider.GetService<IProductRepository>();
            _productService = _serviceProvider.GetService<ProductService>();

            


            Account account = new Account();
            account.Email = Guid.NewGuid().ToString()+"@gmail.com";
            account.Password = Guid.NewGuid().ToString().Substring(0,10);
            _context.Add(account);
             _context.SaveChanges();

            Brand brand = new Brand();
            brand.BrandName=Guid.NewGuid().ToString();
            brand.Description = Guid.NewGuid().ToString();
            brand.AccountId=account.Id;
             _context.Add(brand);

            this.brand = brand;
            this.account = account;



            Category category1 = new Category();
            category1.Name =Guid.NewGuid().ToString();
             _context.Add(category1);
            Category category2 = new Category();
            category2.Name = Guid.NewGuid().ToString();
             _context.Add(category2);
            Category category3 = new Category();
            category3.Name = Guid.NewGuid().ToString();
             _context.Add(category3);

            this.category1 = category1;
            this.category2 = category2;
            this.category3 = category3;

             _context.SaveChanges();

        }
        [TestCleanup]
        public void TestCleanup()
        {


            //_context.Remove(account);
            //_context.Remove(brand);
            //_context.Remove(category1);
            //_context.Remove(category2);
            //_context.Remove(category3);
            //_context.SaveChanges();
        }
    }
}
