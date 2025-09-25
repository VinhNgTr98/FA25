using Microsoft.AspNetCore.Mvc;
using Payment_API.DTOs;
using Payment_API.Services.Interfaces;

namespace Payment_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}", Name = "GetPaymentById")]
        public async Task<ActionResult<PaymentReadDTO>> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetPaymentsByBookingId(Guid bookingId)
        {
            var payments = await _paymentService.GetPaymentsByBookingIdAsync(bookingId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentReadDTO>> CreatePayment([FromBody] PaymentCreateDTO paymentCreateDto)
        {
            var paymentReadDto = await _paymentService.CreatePaymentAsync(paymentCreateDto);

            return CreatedAtRoute(nameof(GetPaymentById), new { id = paymentReadDto.PaymentId }, paymentReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentReadDTO>> UpdatePayment(Guid id, [FromBody] PaymentUpdateDTO paymentUpdateDto)
        {
            var paymentReadDto = await _paymentService.UpdatePaymentAsync(id, paymentUpdateDto);

            if (paymentReadDto == null)
            {
                return NotFound();
            }

            return Ok(paymentReadDto);
        }

        [HttpGet("vnpay-return")]
        public async Task<ActionResult> VnPayReturn()
        {
            var vnpayData = HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            try
            {
                var paymentReadDto = await _paymentService.ProcessVnPayCallbackAsync(vnpayData);

                // Redirect to a frontend success/failure page with payment details
                return Redirect($"/payment-result?status={paymentReadDto.Status}&paymentId={paymentReadDto.PaymentId}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
