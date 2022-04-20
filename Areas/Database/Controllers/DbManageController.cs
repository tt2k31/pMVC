using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/db-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _appDbContext;
        [TempData]
        public string StatusMessage {set; get;}
        public DbManageController(AppDbContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var result = await _appDbContext.Database.EnsureDeletedAsync();

            StatusMessage = result ? "Thanh con" : "Thanh me";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> MigrateAsync()
        {
            await _appDbContext.Database.MigrateAsync();

            StatusMessage = "Thanh con" ;

            return RedirectToAction("Index");
        }
    }
}