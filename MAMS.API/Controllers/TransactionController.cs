using MAMS.API.Data;
using MAMS.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly ApiDataContext _dbContext;

        public TransactionController(ApiDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        [HttpGet("GetReceiptByRef/{refNo}")]
        public async Task<IActionResult> GetReceiptByRef(string refNo)
        {
            var transaction = await _dbContext.Transactions
                .Include(t => t.PatientDetails)
                .Include(t => t.LabResult)
                    .ThenInclude(lr => lr.LabType)
                .FirstOrDefaultAsync(t => t.LabResult.ReferenceNo == refNo);

            if (transaction == null)
                return NotFound("Transaction not found");

            var dto = new TransactionReceiptDto
            {
                ReferenceNo = transaction.LabResult.ReferenceNo,
                PatientName = transaction.PatientDetails.Name,
                LabTestName = transaction.LabResult.LabType.LabName,
                Status = transaction.LabResult.Status,
                BookedDate = transaction.LabResult.BookedDate.ToString("dd/MM/yyyy"),
                TimeSlot = transaction.LabResult.TimeSlot?.ToString(@"hh\:mm") ?? "N/A",
                PaymentMethod = transaction.PaymentMethod.ToString(),
                AmountPaid = transaction.Amount,
                IssuedDate = transaction.Created_Date.ToString("dd/MM/yyyy HH:mm")
            };

            return Ok(dto);
        }
    }
}
