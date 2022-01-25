using DataLayer.Interfaces;
using Domain.APIModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicaLayer
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }
        public List<ProductForPage> GetProductsForPage(int page,int pageSize)
        {
            var x = _productRepository.GetAll();
            return null;
        }

    }
}
