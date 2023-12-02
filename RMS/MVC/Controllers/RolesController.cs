using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Entities;
using MVC.context;

namespace MVC.Controllers
{
	public class RolesController : Controller
	{
		private readonly Db _db;

		public RolesController(Db db)
		{
			_db = db;
		}

		// GET: Roles
		public IActionResult Index()
		{
			return View(_db.Roles.Include(r =>r.Users).OrderBy(r =>r.Name).ToList());

		}

		// GET: Roles/Details/5
		public IActionResult Details(string guid)
		{

			var role = _db.Roles.Include(r => r.Users)
				.SingleOrDefault(m => m.Guid == guid);
			if (role == null)
			{
				return NotFound();
			}

			return View(role);
		}

		// GET: Roles/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Roles/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Role role)
		{
			if (ModelState.IsValid)
			{
				if (!_db.Roles.Any(r => r.Name.ToUpper() == role.Name.ToUpper().Trim()))
				{
					role.Name = role.Name.Trim();
					_db.Add(role);
					_db.SaveChanges();
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", "Role with the same name exists");
			}
			return View(role);
		}

		// GET: Roles/Edit/5
		public IActionResult Edit(string guid)
		{
			var role = _db.Roles.SingleOrDefault(r => r.Guid == guid);
			if (role == null)
			{
				return NotFound();
			}
			return View(role);
		}

		// POST: Roles/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Role role)
		{

			if (ModelState.IsValid)
			{
				if (!_db.Roles.Any(r => r.Name.ToUpper() == role.Name.ToUpper().Trim() && r.Guid != role.Guid))
				{
					role.Name = role.Name.Trim();
					_db.Update(role);
					_db.Update(role);
					_db.SaveChanges();
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", "Role with the same name exists");
			}
			return View(role);
		}

		// GET: Roles/Delete/5
		public IActionResult Delete(string guid)
		{
			
			var role = _db.Roles.Include(r=>r.Users)
				.SingleOrDefault(m=> m.Guid ==guid);
			if (role == null)
			{
				return NotFound();
			}

			return View(role);
		}

		// POST: Roles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(string guid)
		{
		
			var role = _db.Roles.Include(r => r.Users).SingleOrDefault(r=>r.Guid==guid);
			if(role.Users.Count>0)
			{
				ViewBag.Message="Role can`t be deleted because it has users";
				return View("Delete",role);	
			}
	     	_db.Roles.Remove(role);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		private bool RoleExists(int id)
		{
			return (_db.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
