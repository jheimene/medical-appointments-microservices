
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Payment;

namespace PaymentService.Infrastructure.Persistence;

public class PaymentDbContext : DbContext
{

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();

}

