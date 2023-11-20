using mojoin.Models;
using mojoin.Models.Momo;
using ProGCoder_MomoAPI.Models.Order;

namespace mojoin.Services;

public interface IMomoService
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(TransactionHistory model);
    MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
}