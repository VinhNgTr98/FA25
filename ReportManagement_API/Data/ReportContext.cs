using Microsoft.EntityFrameworkCore;
using ReportManagement_API.Models;

namespace ReportManagement_API.Data
{
    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
    }
}
