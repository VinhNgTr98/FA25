using Payment_API.DTOs;

namespace Payment_API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentReadDTO>> GetAllPaymentsAsync();
        Task<PaymentReadDTO> GetPaymentByIdAsync(Guid id);
        Task<PaymentReadDTO> GetPaymentByTransactionNoAsync(string transactionNo);
        Task<IEnumerable<PaymentReadDTO>> GetPaymentsByBookingIdAsync(Guid bookingId);
        Task<PaymentReadDTO> CreatePaymentAsync(PaymentCreateDTO paymentCreateDto);
        Task<PaymentReadDTO> UpdatePaymentAsync(Guid id, PaymentUpdateDTO paymentUpdateDto);
        Task<PaymentReadDTO> ProcessVnPayCallbackAsync(IDictionary<string, string> vnpayData);

    }
}
