using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RidePassAPI.Data;
using RidePassAPI.Dtos.Transaction;
using RidePassAPI.Models;
using RidePassAPI.Responses;

namespace RidePassAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly RidePassAPIContext _context;
        private readonly IHashids _hashids;

        public TransactionsController(RidePassAPIContext context, IHashids hashids)
        {
            _context = context;
            _hashids = hashids;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }
            return await _context.Transaction.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
          if (_context.Transaction == null)
          {
              return NotFound();
          }
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
          if (_context.Transaction == null)
          {
              return Problem("Entity set 'RidePassAPIContext.Transaction'  is null.");
          }
            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (_context.Transaction == null)
            {
                return NotFound();
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getUserTransactions/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetUserTransactions(string userId)
        {
            if (_context.Transaction == null)
            {
                return NotFound();
            }

            var id = _hashids.Decode(userId);
            var transactions = await _context.Transaction.Where(x => x.SenderId == id[0]).ToListAsync();
            var data = transactions.Select(x => new
            {
                Id = _hashids.Encode(x.Id),
                Date = x.CreatedAt,
                Total = x.Amount,
            });
            return Ok(new SuccessResponseWithData<object>(200, "Transactions fetched successfully!", data));
        }

        [HttpPost("makeTransaction")]
        public async Task<ActionResult<ApiResponse>> MakeTransaction(MakeTransactionDto transactionDto)
        {
            var receiverId = _context.Driver.Where(x => x.Username.ToLower() ==  transactionDto.Username.ToLower())
                .FirstOrDefault()?.Id;
            if (receiverId == null)
            {
                return Ok(new ErrorResponse(400, "Driver with username not found!"));
            }

            var senderId = _hashids.Decode(transactionDto.Sender);
            var transaction = new Transaction
            {
                ReceiverId = (int)receiverId,
                SenderId = senderId[0],
                Amount = transactionDto.Amount,
                Description = transactionDto.Description,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
            };

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new SuccessResponse(200, "Transfer has been made successfully!"));
        }

        private bool TransactionExists(int id)
        {
            return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
