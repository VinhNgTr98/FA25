using Payment_API.Models;

namespace Payment_API.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<Payment> GetPaymentByTransactionNoAsync(string transactionNo);
        Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(Guid bookingId);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task<bool> SaveChangesAsync();
    }
}
