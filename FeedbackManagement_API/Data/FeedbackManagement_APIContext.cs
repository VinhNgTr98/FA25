using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FeedbackManagement_API.Model;

namespace FeedbackManagement_API.Data
{
    public class FeedbackManagement_APIContext : DbContext
    {
        public FeedbackManagement_APIContext (DbContextOptions<FeedbackManagement_APIContext> options)
            : base(options)
        {
        }

        public DbSet<FeedbackManagement_API.Model.Feedbacks> Feedback { get; set; } = default!;
    }
}
