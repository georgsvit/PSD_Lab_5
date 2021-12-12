using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSD_Lab_5.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PSD_Lab_5.Services
{
    public class PaymentService
    {
        private readonly LiqpayOptions _options;
        private readonly SessionStorage _storage;

        public PaymentService(IOptions<LiqpayOptions> options, SessionStorage storage)
        {
            _options = options.Value;
            _storage = storage;
        }

        public SignedData CreateSignedData([FromBody]Payment payment)
        {
            var order = new Order(payment, "processing");

            _storage.AddOrder(order);

            var dataObj = new
            {
                public_key = _options.Public,
                version = 3,
                action = "pay",
                amount = payment.Amount,
                currency = payment.Currency,
                description = payment.Description,
                order_id = order.OrderId,

                server_url = "{IP}/payment/post-callback",
                result_url = "http://localhost:8080/about",
            };

            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dataObj)));

            var sign = CreateSign(data);

            return new() 
            {
                Data = data,
                Sign = sign,
            };
        }

        public bool CheckCallback(CallbackModel callback)
        {
            var newSign = CreateSign(callback.Data);

            return newSign == callback.Signature;
        }

        public void ProcessCallback(CallbackModel callback)
        {
            var bytes = Convert.FromBase64String(callback.Data);
            var result = JsonConvert.DeserializeObject<CallbackResult>(Encoding.UTF8.GetString(bytes));

            var oldOrder = _storage.GetOrder(result.Order_id);
            var newOrder = new Order(oldOrder, result.Status);

            _storage.ReplaceOrder(oldOrder, newOrder);
        }

        private string CreateSign(string data)
        {
            var sign_string = $"{_options.Private}{data}{_options.Private}";

            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sign_string));
            var sign = Convert.ToBase64String(hash);

            return sign;
        }
    }
}
