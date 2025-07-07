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

        public ReportController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        [HttpGet("product-report")]
        public IActionResult GetProductReport()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var reportPath = Path.Combine(_env.ContentRootPath, "Reports", "ProductReport.rdlc");

            // SQL থেকে ডাটা ফেচ করা
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT p.Id, p.Name, c.Name AS CategoryName, p.Price, p.StockQuantity, p.CreatedAt, p.Discount 
                                FROM Products p 
                                JOIN Categories c ON p.CategoryId = c.Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            var localReport = new LocalReport(reportPath);
            localReport.AddDataSource("ProductDataSet", dt);

            var result = localReport.Execute(RenderType.Pdf, 1, null, "");
            return File(result.MainStream, "application/pdf", "ProductReport.pdf");
        }
    }
}