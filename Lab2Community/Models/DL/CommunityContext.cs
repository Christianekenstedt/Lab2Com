namespace Lab2Community.Models.DL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CommunityContext : DbContext
    {
        public DbSet<MessageDL> Messages { get; set; }
        public DbSet<UserGroupDL> UserGroups { get; set; }
        public DbSet<UserDL> Users { get; set; }

        public CommunityContext()
            : base("name=CommunityContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
