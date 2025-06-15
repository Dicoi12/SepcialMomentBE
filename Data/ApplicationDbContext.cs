using Microsoft.EntityFrameworkCore;
using SepcialMomentBE.Models;

namespace SepcialMomentBE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventForm> EventForms { get; set; }
        public DbSet<EventFormTemplate> EventFormTemplates { get; set; }
        public DbSet<WeddingExpense> WeddingExpenses { get; set; }
        public DbSet<WeddingGuest> WeddingGuests { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<InvitationTemplate> InvitationTemplates { get; set; }
        public DbSet<WeddingServiceProvider> ServiceProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare pentru User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configurare pentru Event
            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru EventFormTemplate
            modelBuilder.Entity<EventFormTemplate>()
                .HasIndex(t => new { t.EventType, t.FieldName })
                .IsUnique();

            // Configurare pentru EventForm
            modelBuilder.Entity<EventForm>()
                .HasOne(ef => ef.Event)
                .WithMany(e => e.EventForms)
                .HasForeignKey(ef => ef.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventForm>()
                .HasOne(ef => ef.FormTemplate)
                .WithMany()
                .HasForeignKey(ef => ef.FormTemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurare pentru WeddingExpense
            modelBuilder.Entity<WeddingExpense>()
                .HasOne(we => we.Event)
                .WithMany(e => e.WeddingExpenses)
                .HasForeignKey(we => we.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru WeddingGuest
            modelBuilder.Entity<WeddingGuest>()
                .HasOne(wg => wg.Event)
                .WithMany(e => e.WeddingGuests)
                .HasForeignKey(wg => wg.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru Table
            modelBuilder.Entity<Table>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tables)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru InvitationTemplate
            modelBuilder.Entity<InvitationTemplate>()
                .HasOne(it => it.Event)
                .WithMany(e => e.InvitationTemplates)
                .HasForeignKey(it => it.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurare pentru WeddingServiceProvider
            modelBuilder.Entity<WeddingServiceProvider>()
                .HasOne(sp => sp.Event)
                .WithMany(e => e.ServiceProviders)
                .HasForeignKey(sp => sp.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 