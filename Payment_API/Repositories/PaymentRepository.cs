using Microsoft.EntityFrameworkCore;
using Payment_API.Data;
using Payment_API.Models;
using Payment_API.Repositories.Interfaces;
using System;

namespace Payment_API.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(Guid id)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment> GetPaymentByTransactionNoAsync(string transactionNo)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionNo == transactionNo);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(Guid bookingId)
        {
            return await _context.Payments
                .Where(p => p.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            await _context.Payments.AddAsync(payment);
        }

        public Task UpdatePaymentAsync(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            payment.UpdatedAt = DateTime.UtcNow;
            _context.Payments.Update(payment);

            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
