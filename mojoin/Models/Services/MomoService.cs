using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using mojoin.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using mojoin.Models.Momo;
using RestSharp;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ProGCoder_MomoAPI.Models.Order;
using XAct.Library.Settings;

namespace mojoin.Services;

public class MomoService : IMomoService
{
    private readonly DbmojoinContext _dbmojoinContext;
    private readonly IOptions<MomoOptionModel> _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MomoService(IOptions<MomoOptionModel> options, DbmojoinContext dbmojoinContext, IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _dbmojoinContext = dbmojoinContext ?? throw new ArgumentNullException(nameof(dbmojoinContext));
        _httpContextAccessor = httpContextAccessor;
    }
    public class PaymentResponse
    {
        public string PayUrl { get; set; }
        public string Note { get; set; }
    }
    public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(TransactionHistory model)
    {
        // Tạo OrderId từ Timestamp
        model.TransactionReference = DateTime.UtcNow.Ticks.ToString();

        // Tạo thông tin đơn hàng
        model.Note = $"Nội dung: {model.Note}";

        // Tạo dữ liệu để gửi đến Momo
        var rawData =
            $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.TransactionReference}&amount={model.Amount}&orderId={model.TransactionReference}&orderInfo={model.Note}&returnUrl={_options.Value.ReturnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

        // Tính chữ ký
        var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

        // Gửi yêu cầu thanh toán đến Momo
        var client = new RestClient(_options.Value.MomoApiUrl);
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        var requestData = new
        {
            accessKey = _options.Value.AccessKey,
            partnerCode = _options.Value.PartnerCode,
            requestType = _options.Value.RequestType,
            notifyUrl = _options.Value.NotifyUrl,
            returnUrl = _options.Value.ReturnUrl,
            orderId = model.TransactionReference,
            amount = model.Amount.ToString(),
            orderInfo = model.Note,
            requestId = model.TransactionReference,
            extraData = "",         
            signature = signature
        };

        request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);

        //return responseModel;
        return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
    }


    public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
    {
        var amount = collection.First(s => s.Key == "amount").Value;
        var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
        var orderId = collection.First(s => s.Key == "orderId").Value;
        var message = collection.First(s => s.Key == "message").Value;
        //var localmessage = collection.First(s => s.Key == "localmessage").Value;
        return new MomoExecuteResponseModel()
        {
            Amount = amount,
            MoMoId = orderId,
            Message = message,
            OrderInfo = orderInfo
        };
    }

    private string ComputeHmacSha256(string message, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        byte[] hashBytes;

        using (var hmac = new HMACSHA256(keyBytes))
        {
            hashBytes = hmac.ComputeHash(messageBytes);
        }

        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashString;
    }
}