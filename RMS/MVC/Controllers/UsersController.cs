using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC.context;
using MVC.Entities;
using MVC.Enums;

namespace MVC.Controllers
{
	public class UsersController : Controller
	{
		private readonly Db _db;

		public UsersController(Db db) {
			_db = db;
		}

		public IActionResult Index()
		{
			var  userEntities=_db.Users.Include(user=>user.Role).OrderByDescending(user=>user.IsActive).ThenBy(user=>user.Birthdate).ThenBy(user=>user.UserName).ToList();
			return View(userEntities);
		}
		public IActionResult Details(int id)
		{
			

			/*var userEntity = _db.Users.Include(u => u.Role).First(u = u.Id == id);*///kaydı bulamzsa exception fırlatır bulursa last methodu da bulduğu son kaydı döner


			//var userEntity=_db.Users.Include(u=>u.Role).Where(u=>u.Id== id).SingleOrDefault();
			var userEntity = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.Id == id);//Kaydı bulamzsa null döner birden çok kayıt bulursa exception fırlatır.
			
			if(userEntity is null)
			{
				return NotFound("kullanıcı yok");
			}
			return View(userEntity);
		}
		[HttpGet]//Action method(attribute),yazılmazsa default`u get zanneder 

		public IActionResult Create()
		{
			//ViewBag.Roles =new SelectList(_db.Roles.ToList(),"Id","Name");1.yöntem
			ViewData["Roles"] =new SelectList(_db.Roles.OrderBy(r=>r.Name).ToList(),"Id","Name");//2.yöntem
			ViewData["Roles"] =new SelectList(_db.Roles.OrderByDescending(r=>r.Name).ToList(),"Id","Name");//OrderByDescending azalan sıralama yapar
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		//public IActionResult Create(string UserName,string Password,bool IsActive,Statuses Status, DateTime? Birthdate,int RoleId)
		public IActionResult Create(User user)
		{
			if (ModelState.IsValid)
			{
				//	var existingUser = _db.Users.SingleOrDefault(u => u.UserName.ToUpper()==user.UserName.ToUpper().Trim());
				//if(!_db.Users.Any(u=>u.UserName.ToUpper()==user.UserName.ToUpper().Trim()))
				var userNameSqlParameter = new SqlParameter("UserName", user.UserName.Trim());
				var query=_db.Users.FromSqlRaw("select * from Users where Upper(UserName)=UPPER(@UserName)", userNameSqlParameter);
				if(!query.Any())
				{
					user.Guid = Guid.NewGuid().ToString();
					user.UserName = user.UserName.Trim();
					user.Password = user.Password.Trim();
					_db.Users.Add(user);
					_db.SaveChanges();
					TempData["Message"] = "User Created successfully";
					//return RedirectToAction("Index");
					return RedirectToAction(nameof(Index));
				}
				//ViewBag.CreateMessage = "User can`t be created because use with the same user name exists!";
				ModelState.AddModelError("", "User can`t be created because use with the same user name exists!");
				
			}
			ViewBag.Roles = new SelectList(_db.Roles.OrderBy(r => r.Name).ToList(), "Id", "Name");
			return View(user);
		}
		public IActionResult Edit(int id)
		{
			var userEntity=_db.Users.Find(id);
			if(userEntity is null)
			{
				return NotFound();
			}
			ViewBag.Roles = new SelectList(_db.Roles.OrderBy(r => r.Name).ToList(), "Id", "Name");
			return View(userEntity);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(User user)
		{
			if(ModelState.IsValid)
			{
				if (!_db.Users.Any(u => u.UserName.ToUpper() == user.UserName.ToUpper().Trim() && u.Id != user.Id))
				{


					//1.yöntem
					//User existingUser = _db.Users.Find(user.Id);
					//if(existingUser is null)
					//{
					//	return NotFound();
					//}
					//existingUser.UserName = user.UserName.Trim();
					//existingUser.Password = user.Password.Trim();
					//existingUser.IsActive = user.IsActive;
					//existingUser.Status = user.Status;
					//existingUser.Birthdate = user.Birthdate;
					//existingUser.RoleId = user.RoleId;

					//2.yöntem
					user.UserName = user.UserName.Trim();
					user.Password = user.Password.Trim();
					_db.Update(user);
					_db.SaveChanges();
					TempData["message"] = "User updated successfuly";
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", "User can`t be  Updated because use with the same user name exists!");
			}
			ViewBag.Roles = new SelectList(_db.Roles.OrderBy(r => r.Name).ToList(), "Id", "Name");
			return View(user);
		}

		public IActionResult Delete(int id)
		{
			User user = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.Id == id);
			if (user is null)
				return NotFound();
			return View(user);

		}
		[HttpPost,ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			User user = _db.Users.Find(id);
			if (user is null)
				return NotFound();
			_db.Users.Remove(user);
			_db.SaveChanges();
			TempData["message"] = "User deleted successfuly";
			return RedirectToAction(nameof(Index));
		}
	}
}
