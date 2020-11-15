using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MejjHonda.Models;

namespace MejjHonda.Controllers
{
    public class MonedaController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: Moneda
        public ActionResult Index()
        {
            return View(db.MEJJ_Moneda.ToList());
        }

        // GET: Moneda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Moneda mEJJ_Moneda = db.MEJJ_Moneda.Find(id);
            if (mEJJ_Moneda == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Moneda);
        }

        // GET: Moneda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Moneda/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMoneda,Codigo,Nombre")] MEJJ_Moneda mEJJ_Moneda)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_Moneda.Add(mEJJ_Moneda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mEJJ_Moneda);
        }

        // GET: Moneda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Moneda mEJJ_Moneda = db.MEJJ_Moneda.Find(id);
            if (mEJJ_Moneda == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Moneda);
        }

        // POST: Moneda/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMoneda,Codigo,Nombre")] MEJJ_Moneda mEJJ_Moneda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_Moneda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mEJJ_Moneda);
        }

        // GET: Moneda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Moneda mEJJ_Moneda = db.MEJJ_Moneda.Find(id);
            if (mEJJ_Moneda == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Moneda);
        }

        // POST: Moneda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_Moneda mEJJ_Moneda = db.MEJJ_Moneda.Find(id);
            db.MEJJ_Moneda.Remove(mEJJ_Moneda);
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
