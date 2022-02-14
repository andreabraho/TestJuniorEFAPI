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

        /// <summary>
        /// create a product and all data of the product from repo matches with the created product
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateProduct_Success()
        {
            //preparation
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

            //execution
             await _productService.AddProduct(product,catIds);

            //control
            var prodFromRepo= _productRepository.GetById(product.Id).Include(x=>x.ProductCategories).Include(x=>x.Brand).FirstOrDefault();


            Assert.IsNotNull(prodFromRepo);
            Assert.AreEqual(product.Id,prodFromRepo.Id);
            Assert.AreEqual(product.Name,prodFromRepo.Name);
            Assert.AreEqual(product.Description,prodFromRepo.Description);
            Assert.AreEqual(product.Price,prodFromRepo.Price);
            Assert.AreEqual(product.BrandId,prodFromRepo.BrandId);
            Assert.IsTrue(prodFromRepo.ProductCategories.ToList().Select(x=>x.CategoryId).Contains(category1.Id));
            Assert.IsTrue(prodFromRepo.ProductCategories.ToList().Select(x=>x.CategoryId).Contains(category2.Id));
            Assert.IsTrue(prodFromRepo.ProductCategories.ToList().Select(x=>x.CategoryId).Contains(category3.Id));

            _context.Remove(product);
            await _context.SaveChangesAsync();

        }
        /// <summary>
        /// tries to create a null product get exception
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateNullProduct_Failure()
        {

            var x =await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _productService.AddProduct(null, null));
            Assert.AreEqual("product",x.ParamName);   

        }
        /// <summary>
        /// creates a correct product but with null categories
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateProductCatNull_Failure()
        {
            Product product = new Product();
            product.Name = Guid.NewGuid().ToString();
            product.Description = Guid.NewGuid().ToString();
            product.Price = 101010010;
            product.BrandId = brand.Id;
            product.ShortDescription = Guid.NewGuid().ToString();


            var x = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _productService.AddProduct(product, null));
            Assert.AreEqual("categories", x.ParamName);

        }

        [TestMethod]
        public async Task DeleteProduct_Success()
        {
            //preparation
            Product product = new Product();
            product.Name = Guid.NewGuid().ToString();
            product.Description = Guid.NewGuid().ToString();
            product.Price = 101010010;
            product.BrandId = brand.Id;
            product.ShortDescription = Guid.NewGuid().ToString();
            int[] catIds = new int[3];
            catIds[0] = category1.Id;
            catIds[1] = category2.Id;
            catIds[2] = category3.Id;

            await _productService.AddProduct(product, catIds);

            var prodFromRepo = _productRepository.GetById(product.Id).Include(x => x.ProductCategories).Include(x => x.Brand).FirstOrDefault();

            InfoRequest infoRequest1 = new InfoRequest
            {
                Cap = Guid.NewGuid().ToString().Substring(1, 5),
                City = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                InsertDate = DateTime.Now,
                Name = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                ProductId=prodFromRepo.Id,
                RequestText = Guid.NewGuid().ToString(),

            };
            await _context.AddAsync(infoRequest1);
            await _context.SaveChangesAsync();
            InfoRequest infoRequest2 = new InfoRequest
            {
                Cap = Guid.NewGuid().ToString().Substring(1, 5),
                City = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                InsertDate = DateTime.Now,
                ProductId=prodFromRepo.Id,
                Name = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                RequestText = Guid.NewGuid().ToString(),

            };
            await _context.AddAsync(infoRequest2);
            await _context.SaveChangesAsync();
            InfoRequestReply infoRequestReply1 = new InfoRequestReply
            {
                InsertDate = DateTime.Now,
                AccountId = account.Id,
                InfoRequestId = infoRequest1.Id,
                ReplyText = Guid.NewGuid().ToString()
            };
            await _context.AddAsync(infoRequestReply1);
            await _context.SaveChangesAsync();
            InfoRequestReply infoRequestReply2 = new InfoRequestReply
            {
                InsertDate = DateTime.Now,
                AccountId = account.Id,
                InfoRequestId = infoRequest1.Id,
                ReplyText = Guid.NewGuid().ToString()
            };
            await _context.AddAsync(infoRequestReply2);
            await _context.SaveChangesAsync();
            InfoRequestReply infoRequestReply3 = new InfoRequestReply
            {
                InsertDate = DateTime.Now,
                AccountId = account.Id,
                InfoRequestId = infoRequest2.Id,
                ReplyText = Guid.NewGuid().ToString()
            };
            await _context.AddAsync(infoRequestReply3);
            await _context.SaveChangesAsync();
            InfoRequestReply infoRequestReply4 = new InfoRequestReply
            {
                InsertDate = DateTime.Now,
                AccountId = account.Id,
                InfoRequestId = infoRequest2.Id,
                ReplyText = Guid.NewGuid().ToString()
            };
            await _context.AddAsync(infoRequestReply4);
            await _context.SaveChangesAsync();


            




        }













        [TestInitialize]
        public async Task TestInitializer()
        {
            _context = _serviceProvider.GetService<MyContext>();
            _productRepository = _serviceProvider.GetService<IProductRepository>();
            _productService = _serviceProvider.GetService<ProductService>();

            


            Account account = new Account();
            account.Email = Guid.NewGuid().ToString()+"@gmail.com";
            account.Password = Guid.NewGuid().ToString().Substring(0,10);
            await _context.AddAsync(account);
            await _context.SaveChangesAsync();

            Brand brand = new Brand();
            brand.BrandName=Guid.NewGuid().ToString();
            brand.Description = Guid.NewGuid().ToString();
            brand.AccountId=account.Id;
            await _context.AddAsync(brand);
            await _context.SaveChangesAsync();

            this.brand = brand;
            this.account = account;



            Category category1 = new Category();
            category1.Name =Guid.NewGuid().ToString();
            await _context.AddAsync(category1);
            Category category2 = new Category();
            category2.Name = Guid.NewGuid().ToString();
            await _context.AddAsync(category2);
            Category category3 = new Category();
            category3.Name = Guid.NewGuid().ToString();
            await _context.AddAsync(category3);
            await _context.SaveChangesAsync();


            this.category1 = category1;
            this.category2 = category2;
            this.category3 = category3;

             

        }
        [TestCleanup]
        public async Task TestCleanup()
        {

            _context.Remove(brand);
            _context.Remove(account);
            _context.Remove(category1);
            _context.Remove(category2);
            _context.Remove(category3);
            await _context.SaveChangesAsync();
        }
    }
}
