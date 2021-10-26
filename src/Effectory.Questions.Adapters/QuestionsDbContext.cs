using Effectory.Questions.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questions.Adapters
{
    public class QuestionsDbContext : DbContext
    {
        public QuestionsDbContext(ISendMessages messageSender, DbContextOptions<QuestionsDbContext> options) : base(options)
        {

        }

        public DbSet<QuestionEntity> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

}