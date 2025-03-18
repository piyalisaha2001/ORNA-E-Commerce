using e_commerce.Data;
using e_commerce.DTO;
using e_commerce.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Payment
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PaymentsDTO>>> GetPayments()
        {
            var payments = await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();

            var paymentsDto = payments.Select(p => new PaymentsDTO
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                PaymentMethod = p.PaymentMethod,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate
            }).ToList();

            return Ok(paymentsDto);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaymentsDTO>> GetPayment(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
            {
                return NotFound();
            }

            var paymentDto = new PaymentsDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate
            };

            return Ok(paymentDto);
        }

        // POST: api/Payment
        //[HttpPost("MakePayment")]
        //[Authorize(Roles = "User")]
        //public async Task<ActionResult<PaymentsDTO>> PostPayment(PaymentsDTO paymentDto)
        //{
        //    var payment = new Payments
        //    {
        //        OrderId = paymentDto.OrderId,
        //        PaymentMethod = paymentDto.PaymentMethod,
        //        Amount = paymentDto.Amount,
        //        PaymentDate = paymentDto.PaymentDate
        //    };

        //    _context.Payments.Add(payment);
        //    await _context.SaveChangesAsync();

        //    paymentDto.PaymentId = payment.PaymentId;

        //    return CreatedAtAction(nameof(GetPayment), new { id = paymentDto.PaymentId }, paymentDto);
        //}
    }
}