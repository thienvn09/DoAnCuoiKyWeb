using DoAn.Models;
using DoAn.Models.Momo;

namespace DoAn.Service
{
    public interface IMomoService 
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);

    }
}
