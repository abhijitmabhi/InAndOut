using InAndOut.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InAndOut.Models;
using InAndOut.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InAndOut.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ExpenseController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Expense> objList = _db.Expenses;
            foreach (var obj in objList)
            {
                obj.Category = _db.Categories.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            return View(objList);
        }

        public IActionResult Create()
        {
            // IEnumerable<SelectListItem> categoryType = _db.Categories.Select(i => new SelectListItem
            // {
            //     Text = i.CategoryName,
            //     Value = i.Id.ToString()
            // });

            var expenseVm = new ExpenseVm()
            {
                Expense = new Expense(),
                TypeDropDown = _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                })
            };

            // ViewBag.TypeDorpdown = categoryType;

            return View(expenseVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseVm obj)
        {
            if (!ModelState.IsValid) return View(obj);
            _db.Expenses.Add(obj.Expense);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Expenses.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Expenses.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Expenses.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Expenses.Find(id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Expense obj)
        {
            if (!ModelState.IsValid) return View(obj);
            _db.Expenses.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
