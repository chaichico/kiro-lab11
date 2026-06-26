using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wildrydes.net.Context;
using wildrydes.net.Models;
using wildrydes.net.Decorators;

namespace wildrydes.net.Controllers
{
    [Protected]
    public class UnicornController : Controller
    {
        private DefaultContext db = new DefaultContext();

        // GET: Unicorn
        public ActionResult Index()
        {
            return View(db.Unicorns.ToList());
        }

        // GET: Unicorn/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unicorn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Rating")] UnicornModel unicornModel)
        {
            if (ModelState.IsValid)
            {
                unicornModel.Id = Guid.NewGuid();
                db.Unicorns.Add(unicornModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unicornModel);
        }

        // GET: Unicorn/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnicornModel unicornModel = db.Unicorns.Find(id);
            if (unicornModel == null)
            {
                return HttpNotFound();
            }
            return View(unicornModel);
        }

        // POST: Unicorn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnicornModel unicornModel = db.Unicorns.Find(id);
            db.Unicorns.Remove(unicornModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
