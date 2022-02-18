using Domain;
using ServicaLayer.ProductService.Model;
using System.Threading.Tasks;

namespace ServicaLayer.ProductService
{
    public interface IProductService
    {
        Task<bool> DeleteProduct(int id);
        GetInsertProductDTO GetInsertProductDTO();
        Task<ProductDetailDTO> GetProductDetail(int id);
        GetUpdateProductDTO GetProductForUpdate(int id);
        ProductPageDTO GetProductsForPage(int page, int pageSize, int brandId = 0, OrderProduct orderBy = OrderProduct.BrandName, bool isAsc = true);
        Task<int> UpsertProduct(Product product, int[] categories);
    }
}