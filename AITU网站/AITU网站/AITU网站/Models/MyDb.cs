namespace AITU网站.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDb : DbContext
    {
        public MyDb()
            : base("name=MyDb")
        {
        }

        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>()
                .Property(e => e.UserId)
                .IsFixedLength();

            modelBuilder.Entity<Collection>()
                .Property(e => e.ImgID)
                .IsFixedLength();

            modelBuilder.Entity<Image>()
                .Property(e => e.ImgId)
                .IsFixedLength();

            modelBuilder.Entity<Image>()
                .Property(e => e.UserId)
                .IsFixedLength();

            modelBuilder.Entity<Image>()
                .HasMany(e => e.Collection)
                .WithRequired(e => e.Image)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserId)
                .IsFixedLength();
        }
    }
}
