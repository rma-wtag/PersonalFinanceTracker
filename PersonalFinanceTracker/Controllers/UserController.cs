using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PersonalFinanceTracker.Dtos.UserDtos;
using PersonalFinanceTracker.Entities;
using PersonalFinanceTracker.Mappers;
using PersonalFinanceTracker.Models;
using System.Text;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetStudentById([FromRoute] int id) {
            var student = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (student == null) {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto) {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = userDto.ToUserFromCreateDto();
            await _dbContext.Users.AddAsync(userModel);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = userModel.UserId }, userModel);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateUserDto userDto) {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();

            user.UserName = userDto.UserName;
            user.Balance = userDto.Balance;

            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }
        [HttpDelete("{id:int}")]
        public async Task<User?> Delete([FromRoute] int id) {
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else return null;

            return user;
        }

        [HttpGet("{userId}/generateInvoice")]
        public async Task<IActionResult> GenerateTransactionInvoicePdf([FromRoute]int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Transactions)
                    .ThenInclude(t => t.Category)
                .Include(u => u.Transactions)
                    .ThenInclude(t => t.Payment)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.Transactions == null || !user.Transactions.Any())
                return NotFound("No transactions found.");

            string htmlContent = GenerateHtml(user);

            // Generate PDF using IronPdf
            var renderer = new IronPdf.HtmlToPdf();
            var pdfDoc = renderer.RenderHtmlAsPdf(htmlContent);
            byte[] pdfBytes = pdfDoc.BinaryData;

            return File(pdfBytes, "application/pdf", $"Transactions_{user.UserName}.pdf");
        }

        private string GenerateHtml(User user)
        {
            var sb = new StringBuilder();

            sb.Append($@"
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; }}
            h1 {{ text-align: center; }}
            table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
            th, td {{ border: 1px solid #ccc; padding: 8px; text-align: center; }}
            th {{ background-color: #f4f4f4; }}
        </style>
    </head>
    <body>
        <h1>Transaction Report for {user.UserName}</h1>
        <p><strong>Current Balance:</strong> {user.Balance:C}</p>
        <table>
            <tr>
                <th>#</th>
                <th>Description</th>
                <th>Category</th>
                <th>Type</th>
                <th>Amount</th>
                <th>Status</th>
                <th>Date</th>
            </tr>");

            int count = 1;
            foreach (var tx in user.Transactions.OrderBy(t => t.Date))
            {
                sb.Append($@"
            <tr>
                <td>{count++}</td>
                <td>{tx.Description}</td>
                <td>{tx.Category?.Name}</td>
                <td>{tx.Category?.PaymentType}</td>
                <td>{tx.Amount:C}</td>
                <td>{tx.Payment?.Status}</td>
                <td>{tx.Date:yyyy-MM-dd}</td>
            </tr>");
            }

            sb.Append("</table></body></html>");
            return sb.ToString();
        }
    }
}