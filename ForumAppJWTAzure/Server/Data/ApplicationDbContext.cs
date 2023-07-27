namespace ForumAppJWTAzure.Server.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Forum> Forums { get; set; }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<ForumTag> ForumTags { get; set; }

        public virtual DbSet<Vote> Votes { get; set; }

        public virtual DbSet<AppLog> AppLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER",
                        Id = "8343074e-8623-4e1a-b0c1-84fb8678c8f3",
                    },
                    new IdentityRole
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR",
                        Id = "c7ac6cfe-1f10-4baf-b604-cde350db9554",
                    });

            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "8e448afa-f008-446e-a52f-13c449803c2e",
                    Email = "admin@bookstore.com",
                    NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                    UserName = "admin@bookstore.com",
                    NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                    DisplayName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),                    
                },
                new ApplicationUser
                {
                    Id = "30a24107-d279-4e37-96fd-01af5b38cb27",
                    Email = "user@bookstore.com",
                    NormalizedEmail = "USER@BOOKSTORE.COM",
                    UserName = "user@bookstore.com",
                    NormalizedUserName = "USER@BOOKSTORE.COM",
                    DisplayName = "User",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),                    
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "8343074e-8623-4e1a-b0c1-84fb8678c8f3",
                    UserId = "30a24107-d279-4e37-96fd-01af5b38cb27",
                },
                new IdentityUserRole<string>
                {
                    RoleId = "c7ac6cfe-1f10-4baf-b604-cde350db9554",
                    UserId = "8e448afa-f008-446e-a52f-13c449803c2e",
                });

            builder.Entity<Forum>().HasMany(x => x.Tags)
                    .WithMany(x => x.Forums)
                    .UsingEntity<ForumTag>(
                        x => x.HasOne(x => x.Tag)
                        .WithMany().HasForeignKey(x => x.TagId),
                        x => x.HasOne(x => x.Forum)
                        .WithMany().HasForeignKey(x => x.ForumId));

            this.OnModelCreatingPartial(builder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime createdDate = DateTime.UtcNow;

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).ModifiedDate = createdDate;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = createdDate;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
