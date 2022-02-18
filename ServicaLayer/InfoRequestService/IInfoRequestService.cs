using ServicaLayer.InfoRequestService.Model;
using System.Threading.Tasks;

namespace ServicaLayer.InfoRequestService
{
    public interface IInfoRequestService
    {
        Task<InfoRequestDetailDTO> GetInfoRequestDetail(int id);
        InfoRequestPageDTO GetPage(int page, int pageSize, int idBrand = 0, string productNameSearch = null, bool isAsc = true, int productId = 0);
    }
}