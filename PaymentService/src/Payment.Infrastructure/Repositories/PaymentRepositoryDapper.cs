using Dapper;
using Npgsql;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Payment;
using PaymentService.Domain.Payment.Enums;

namespace PaymentService.Infrastructure.Repositories
{
    public class PaymentRepositoryDapper : IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepositoryDapper(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<int> CreateAsync(Payment payment)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"INSERT INTO sch_payment.payment(
	                    method_id, currency, amount, order_id, order_number, customer_id, customer_fullname, status, created_date, created_by)
	                    VALUES (@Method, @Currency, @Amount, @OrderId, @OrderNumber, @CustomerId, @CustomerFullName, @Status, @CreatedDate, @CreatedBy)
                    RETURNING payment_id";


                return await conn.ExecuteScalarAsync<int>(sql, new
                {
                    Method = (int)payment.Provider,
                    payment.Currency,
                    payment.Amount,
                    payment.OrderId,
                    payment.OrderNumber,
                    payment.CustomerId,
                    payment.CustomerFullName,
                    Status = payment.Status.ToString(),
                    payment.CreatedDate,
                    payment.CreatedBy
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            } 
        }

        public async Task<IEnumerable<PaymentResponse>?> GetByCustomerIdAsync(Guid customerId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = """
                SELECT P.payment_id AS PaymentId, M.name AS Method, P.amount AS Amount, P.order_id AS OrderId, P.order_number AS OrderNumber, 
                    P.customer_id AS CustomerId, P.customer_fullname AS CustomerFullName, P.created_date AS CreatedDate 
                FROM sch_payment.payment P 
                INNER JOIN sch_payment.method M ON M.method_id = P.method_id 
                WHERE P.customer_id = @customerId
            """;
            return await conn.QueryAsync<PaymentResponse>(sql, new { customerId });
        }

        public async Task<PaymentResponse?> GetByOrderIdAsync(Guid orderId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = """
                SELECT P.payment_id AS PaymentId, M.name AS Method, P.amount AS Amount, P.order_id AS OrderId, P.order_number AS OrderNumber, 
                    P.customer_id AS CustomerId, P.customer_fullname AS CustomerFullName, P.created_date AS CreatedDate 
                FROM sch_payment.payment P 
                INNER JOIN sch_payment.method M ON M.method_id = P.method_id 
                WHERE P.order_id = @orderId
            """;
            return await conn.QueryFirstOrDefaultAsync<PaymentResponse>(sql, new { orderId });
        }

        public async Task UpdateStatusAsync(int paymentId, PaymentStatus status, string externalPaymentId, string extra, string userIdModified, DateTime modifiedDate)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"
            UPDATE sch_payment.payment
            SET status = @Status, external_payment_id = COALESCE(@ExternalId, external_payment_id), 
            extra = COALESCE(@Extra, extra), modified_by = @UserIdModified, modified_date = @ModifiedDate
            WHERE payment_id = @PaymentId";

            await conn.ExecuteAsync(sql, new
            {
                PaymentId = paymentId,
                Status = status.ToString(),
                ExternalId = externalPaymentId,
                Extra = extra,
                UserIdModified = userIdModified,
                ModifiedDate = modifiedDate
            });
        }
    }
}
