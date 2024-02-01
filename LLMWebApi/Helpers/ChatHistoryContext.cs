using LLMWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LLMWebApi 
{
    public class ChatHistoryContext : DbContext 
    {
        // Enitity set for chatMeesage table
        public DbSet<ChatMessage> ChatMessages {get; set;}
        public string DbPath {get;}

        public ChatHistoryContext()
        {
            DbPath = Path.Join(Environment.CurrentDirectory, "db", "chatHistory.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessage");
        }
    }
}