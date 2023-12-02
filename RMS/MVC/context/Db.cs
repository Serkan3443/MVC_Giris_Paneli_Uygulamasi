using Microsoft.EntityFrameworkCore;
using MVC.Entities;

namespace MVC.context
{
	public class Db:DbContext
	{
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<UserResource> userResources { get; set; }

        public Db(DbContextOptions options) : base(options)
        {

        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//many to many ilişkiler için yapılmalı
			modelBuilder.Entity<UserResource>().HasKey(e => new { e.UserId, e.ResourceId });

			modelBuilder.Entity<User>().HasOne(user => user.Role)
				.WithMany(role => role.Users)
				.HasForeignKey(user => user.RoleId)//opsiyonel
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserResource>().HasOne(userResource => userResource.User)
				.WithMany(User => User.UserResources)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserResource>().HasOne(userResources => userResources.Resource)
				.WithMany(resource => resource.UserResources)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Resource>().HasIndex(resourse => resourse.Date);

			modelBuilder.Entity<User>().HasIndex(user => user.UserName).IsUnique();
		}

        internal object OrderBy(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
