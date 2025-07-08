using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace InventoryManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IWebHostEnvironment env, IConfiguration configuration, ILogger<ReportController> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("product-report")]
        public IActionResult GetProductReport()
        {
            string reportPath = Path.Combine(_env.WebRootPath, "Reports", "ProductReport.rdlc"); // ইনিশিয়ালাইজেশন সরাসরি করা
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // RDLC ফাইলের অস্তিত্ব চেক
                if (!System.IO.File.Exists(reportPath))
                {
                    _logger.LogError("RDLC report file not found at: {ReportPath}", reportPath);
                    return StatusCode(500, new { error = $"Report template file not found at {reportPath}." });
                }

                // SQL থেকে ডাটা ফেচ করা
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //string query = @"SELECT p.Id, p.Name, c.Name AS CategoryName, p.Price, p.StockQuantity, p.CreatedAt, p.Discount 
                    //               FROM Products p 
                    //               LEFT JOIN Categories c ON p.CategoryId = c.Id";
                    string query = @"SELECT p.Id, p.Name, c.Name AS CategoryName, p.CategoryId, p.Price, p.StockQuantity, p.CreatedAt, p.Discount 
                                    FROM Products p 
                                    LEFT JOIN Categories c ON p.CategoryId = c.Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    _logger.LogWarning("No data found for product report.");
                    return NotFound(new { error = "No data available for report generation." });
                }

                var localReport = new LocalReport(reportPath);
                localReport.AddDataSource("ProductDataSet", dt);

                var result = localReport.Execute(RenderType.Pdf, 1, null, "");
                return File(result.MainStream, "application/pdf", "ProductReport.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating product report at path: {ReportPath}", reportPath);
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}