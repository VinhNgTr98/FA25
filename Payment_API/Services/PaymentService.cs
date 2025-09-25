using AutoMapper;
using Payment_API.DTOs;
using Payment_API.Helpers;
using Payment_API.Models;
using Payment_API.Repositories.Interfaces;
using Payment_API.Services.Interfaces;

namespace Payment_API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly VNPayHelper _vnpayHelper;
        private readonly IConfiguration _configuration;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, VNPayHelper vnpayHelper, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _vnpayHelper = vnpayHelper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PaymentReadDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
        }

        public async Task<PaymentReadDTO> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            return _mapper.Map<PaymentReadDTO>(payment);
        }

        public async Task<PaymentReadDTO> GetPaymentByTransactionNoAsync(string transactionNo)
        {
            var payment = await _paymentRepository.GetPaymentByTransactionNoAsync(transactionNo);
            return _mapper.Map<PaymentReadDTO>(payment);
        }

        public async Task<IEnumerable<PaymentReadDTO>> GetPaymentsByBookingIdAsync(Guid bookingId)
        {
            var payments = await _paymentRepository.GetPaymentsByBookingIdAsync(bookingId);
            return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
        }

        public async Task<PaymentReadDTO> CreatePaymentAsync(PaymentCreateDTO paymentCreateDto)
        {
            var paymentModel = _mapper.Map<Payment>(paymentCreateDto);
            paymentModel.PaymentId = Guid.NewGuid();
            var returnUrl = _configuration["VNPAY:ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                throw new InvalidOperationException("ReturnUrl is not configured in appsettings.json");
            // Generate VNPay payment URL
            var vnpayPaymentUrl = _vnpayHelper.CreatePaymentUrl(
                paymentModel.PaymentId,
                paymentModel.Amount,
                returnUrl,
                "Payment for booking " + paymentModel.BookingId
            );

            paymentModel.PaymentUrl = vnpayPaymentUrl;

            await _paymentRepository.CreatePaymentAsync(paymentModel);
            await _paymentRepository.SaveChangesAsync();

            return _mapper.Map<PaymentReadDTO>(paymentModel);
        }

        public async Task<PaymentReadDTO> UpdatePaymentAsync(Guid id, PaymentUpdateDTO paymentUpdateDto)
        {
            var paymentModel = await _paymentRepository.GetPaymentByIdAsync(id);
            if (paymentModel == null)
            {
                return null;
            }

            _mapper.Map(paymentUpdateDto, paymentModel);

            await _paymentRepository.UpdatePaymentAsync(paymentModel);
            await _paymentRepository.SaveChangesAsync();

            return _mapper.Map<PaymentReadDTO>(paymentModel);
        }

        public async Task<PaymentReadDTO> ProcessVnPayCallbackAsync(IDictionary<string, string> vnpayData)
        {
            var isValidSignature = _vnpayHelper.ValidateSignature(vnpayData);

            if (!isValidSignature)
            {
                throw new Exception("Invalid VNPay signature");
            }

            // Extract payment ID from vpn_TxnRef
            if (!vnpayData.TryGetValue("vnp_TxnRef", out string txnRef) || !Guid.TryParse(txnRef, out Guid paymentId))
            {
                throw new Exception("Invalid transaction reference");
            }

            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
            {
                throw new Exception("Payment not found");
            }

            // Extract response code
            if (!vnpayData.TryGetValue("vnp_ResponseCode", out string responseCode))
            {
                throw new Exception("Response code not found");
            }

            // Update payment information
            payment.Status = responseCode == "00" ? "Success" : "Failed";
            payment.TransactionNo = vnpayData.TryGetValue("vnp_TransactionNo", out string transactionNo) ? transactionNo : string.Empty;
            payment.BankCode = vnpayData.TryGetValue("vnp_BankCode", out string bankCode) ? bankCode : string.Empty;
            payment.UpdatedAt = DateTime.UtcNow;

            await _paymentRepository.UpdatePaymentAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            return _mapper.Map<PaymentReadDTO>(payment);
        }
    }
}
