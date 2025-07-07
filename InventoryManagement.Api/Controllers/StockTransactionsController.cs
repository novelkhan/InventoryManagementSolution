using InventoryManagement.Common.Dtos;
using InventoryManagement.Core.Entities;
using InventoryManagement.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // শুধুমাত্র লগইন করা ব্যবহারকারী অ্যাক্সেস করতে পারবে
    public class StockTransactionsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public StockTransactionsController(InventoryDbContext context)
        {
            _context = context;
        }

        // স্টক ট্রানজাকশন তালিকা পাওয়া
        [HttpGet]
        public async Task<IActionResult> GetStockTransactions()
        {
            var transactions = await _context.StockTransactions
                .Select(t => new StockTransactionDto
                {
                    Id = t.Id,
                    ProductId = t.ProductId,
                    Quantity = t.Quantity,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate
                }).ToListAsync();

            return Ok(transactions);
        }

        // নতুন স্টক ট্রানজাকশন যোগ করা
        [HttpPost]
        public async Task<IActionResult> AddStockTransaction([FromBody] StockTransactionDto stockTransactionDto)
        {
            if (stockTransactionDto == null)
                return BadRequest("Stock transaction data is required");

            var stockTransaction = new StockTransaction
            {
                ProductId = stockTransactionDto.ProductId,
                Quantity = stockTransactionDto.Quantity,
                TransactionType = stockTransactionDto.TransactionType,
                TransactionDate = stockTransactionDto.TransactionDate
            };

            _context.StockTransactions.Add(stockTransaction);
            await _context.SaveChangesAsync();

            stockTransactionDto.Id = stockTransaction.Id; // আইডি রিটার্ন করা
            return CreatedAtAction(nameof(GetStockTransactions), new { id = stockTransaction.Id }, stockTransactionDto);
        }

        // স্টক ট্রানজাকশন আপডেট করা
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStockTransaction(int id, [FromBody] StockTransactionDto stockTransactionDto)
        {
            if (stockTransactionDto == null || id != stockTransactionDto.Id)
                return BadRequest("Invalid stock transaction data");

            var stockTransaction = await _context.StockTransactions.FindAsync(id);
            if (stockTransaction == null)
                return NotFound();

            stockTransaction.ProductId = stockTransactionDto.ProductId;
            stockTransaction.Quantity = stockTransactionDto.Quantity;
            stockTransaction.TransactionType = stockTransactionDto.TransactionType;
            stockTransaction.TransactionDate = stockTransactionDto.TransactionDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // স্টক ট্রানজাকশন মুছা
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockTransaction(int id)
        {
            var stockTransaction = await _context.StockTransactions.FindAsync(id);
            if (stockTransaction == null)
                return NotFound();

            _context.StockTransactions.Remove(stockTransaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}