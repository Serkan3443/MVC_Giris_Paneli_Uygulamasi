using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.context;
using MVC.Entities;
using System.Globalization;
using System.Linq.Expressions;

namespace MVC.Controllers
{
	public class DbController :Controller
	{
		//bağımlılık yönetimi
		private readonly Db _db;

		public object Users { get; internal set; }

		public DbController(Db db)
        {
			_db = db;
        }

        public IActionResult Test()
		{
			return Content("User count:"+ _db.Users.Count());
		}
		public IActionResult Seed()
		{
			//1.yöntem
			//var userEntities = _db.Users.AsNoTracking().ToList();

			//2.yöntem
			var userEntities = _db.Users.ToList();

			_db.Users.RemoveRange(userEntities);

			var roleEntities=_db.Roles.ToList();
			_db.Roles.RemoveRange(roleEntities);

			_db.Resources.RemoveRange(_db.Resources.ToList());
			_db.userResources.RemoveRange(_db.userResources.ToList());
		   
			//1.yöntem
			//if(roleEntities.Count>0)

			//2.yöntem
			if(roleEntities.Any())
			{
				_db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Roles', RESEED,0)");
			}

			_db.Roles.Add(new Role()
			{
				
				Name="admin",
				GuId=Guid.NewGuid().ToString(),
				Users=new List<User>()
				{
					new User()
					{
						Birthdate=new DateTime(1980,5,27),
						IsActive=true,
						Password="cagil",
						Status=Enums.Statuses.Junior,
						UserName="cagil",
					}

				}
			}
				) ;
			_db.Roles.Add(new Role()
			{
				Name="user",
                GuId = Guid.NewGuid().ToString(),
                Users =new List<User>()
				{
					new User()
					{
						Birthdate=DateTime.Parse("20.10.2022",new CultureInfo("tr-TR")),
						IsActive=true,
						Password="leo",
						Status=Enums.Statuses.Master,
						UserName="leo",
					}
				}
			});
			_db.SaveChanges();

			_db.Resources.Add(new Resource()
			{ 
				Content="Primitive data types and variables",
				Date=DateTime.Parse("01/09/2023", new CultureInfo("en-US")),
				Score=4.5,
				Title="C# types and variables",
				UserResources=new List<UserResource>()
				{
					new UserResource()
					{
						UserId=_db.Users.SingleOrDefault(user=>user.UserName=="cagil").Id
					}
				}
			});

			_db.Resources.Add(new Resource()
			{
				Content = "Methods, if, switch and ternary conditionals",
				Date = DateTime.Parse("03/27/2023", new CultureInfo("en-US")),
				Score = 4,
				Title = "C# types and variables",
				UserResources = new List<UserResource>()
				{
					new UserResource()
					{
						UserId=_db.Users.SingleOrDefault(user=>user.UserName=="cagil").Id
					},
					new UserResource()
					{
						UserId=_db.Users.SingleOrDefault(user=>user.UserName=="leo").Id
					}

				}
			});


			_db.Resources.Add(new Resource()
			{
				Content = "Loops and arrays as reference types",
				Date = DateTime.Parse("05/15/2023", new CultureInfo("en-US")),
				Score = 3.5,
				Title = "C# Loops and Collections",
				UserResources = new List<UserResource>()
				{
					new UserResource()
					{
						UserId=_db.Users.SingleOrDefault(user=>user.UserName=="cagil").Id
					},
					new UserResource()
					{
						UserId=_db.Users.SingleOrDefault(user=>user.UserName=="leo").Id
					}

				}
			});


			_db.SaveChanges();
			return Content("Database seed successful. ");
		}
	}
}
