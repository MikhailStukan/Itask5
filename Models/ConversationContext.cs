using System;
using Microsoft.EntityFrameworkCore;

namespace Itask5.Models

{
    public class ConversationContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        public ConversationContext(DbContextOptions<ConversationContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
