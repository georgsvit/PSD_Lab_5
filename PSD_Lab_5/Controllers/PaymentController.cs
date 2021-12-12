using Microsoft.AspNetCore.Mvc;
using PSD_Lab_5.Models;
using PSD_Lab_5.Services;
using System.Linq;

namespace PSD_Lab_5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly SessionStorage _storage;

        public PaymentController(PaymentService paymentService, SessionStorage storage)
        {
            _paymentService = paymentService;
            _storage = storage;
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody]Payment payment)
        {
            var data = _paymentService.CreateSignedData(payment);

            return Ok(data);
        }

        [HttpPost("post-callback")]
        public IActionResult Callback()
        {
            var callback = new CallbackModel() 
            { 
                Data = Request.Form["data"],
                Signature = Request.Form["signature"],
            };

            if (!_paymentService.CheckCallback(callback))
            {
                return BadRequest();
            }

            _paymentService.ProcessCallback(callback);

            return Ok();
        }

        [HttpGet("payments")]
        public IActionResult GetPayments()
        {
            return Ok(_storage.GetOrders().Select(o => new { o.OrderId, o.Status, o.Payment.Amount, o.Payment.Currency, o.Payment.Description }));
        }
    }
}
