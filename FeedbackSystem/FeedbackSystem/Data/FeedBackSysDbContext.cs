using FeedbackSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeedbackSystem.Data
{
    public class FeedBackSysDbContext: DbContext
    {
        //00016599
        public FeedBackSysDbContext(DbContextOptions<FeedBackSysDbContext> options) :base(options) 
        {
            
        }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)           
                .WithMany(u => u.Feedbacks)    
                .HasForeignKey(f => f.UserId)  
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
