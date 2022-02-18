using Domain;
using Domain.ModelsForApi;
using ServicaLayer.BrandService.Model;
using System.Threading.Tasks;

namespace ServicaLayer.BrandService
{
    public interface IBrandService
    {
        Task<bool> DeleteAll(int id);
        Task<bool> EditBrand(Brand brand);
        Task<Brand> GetBrand(int id);
        Task<BrandDetailDTO> GetBrandDetail(int id);
        BrandDetailDTO GetBrandDetail2(int id);
        BrandPageDTO GetBrandPage(int page, int pageSize);
        BrandPageDTO GetBrandPage2(int page, int pageSize);
        Task<int> InsertBrand(Account account, Brand brand, ProdWithCat[] prodsWithCat);
        Task<bool> ValidateExistsEmail(string email);
    }
}