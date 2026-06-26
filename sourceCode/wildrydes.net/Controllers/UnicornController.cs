using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wildrydes.net.Context;
using wildrydes.net.Models;

namespace wildrydes.net.Controllers;

[Authorize]
public class UnicornController : Controller
{
    private readonly DefaultContext _db;

    public UnicornController(DefaultContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _db.Unicorns.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating")] UnicornModel unicornModel)
    {
        if (ModelState.IsValid)
        {
            unicornModel.Id = Guid.NewGuid();
            _db.Unicorns.Add(unicornModel);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(unicornModel);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var unicornModel = await _db.Unicorns.FindAsync(id);
        if (unicornModel == null)
        {
            return NotFound();
        }

        return View(unicornModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var unicornModel = await _db.Unicorns.FindAsync(id);
        if (unicornModel != null)
        {
            _db.Unicorns.Remove(unicornModel);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
