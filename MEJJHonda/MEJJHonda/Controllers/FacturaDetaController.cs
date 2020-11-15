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
    public class FacturaDetaController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: FacturaDeta
        public ActionResult Index()
        {
            var mEJJ_FacturaDeta = db.MEJJ_FacturaDeta.Include(m => m.MEJJ_Articulo).Include(m => m.MEJJ_FacturaEnca);
            return View(mEJJ_FacturaDeta.ToList());
        }

        // GET: FacturaDeta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaDeta mEJJ_FacturaDeta = db.MEJJ_FacturaDeta.Find(id);
            if (mEJJ_FacturaDeta == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_FacturaDeta);
        }

        // GET: FacturaDeta/Create
        public ActionResult Create()
        {
            ViewBag.IdArticulo = new SelectList(db.MEJJ_Articulo, "IdArticulo", "Descripcion");
            ViewBag.IdFacturaE = new SelectList(db.MEJJ_FacturaEnca, "IdFacturaE", "Tipo");
            return View();
        }

        // POST: FacturaDeta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFacturaD,IdFacturaE,IdArticulo,Cantidad,PorcentajeDesc,SubTotal,Descuento,Impuesto")] MEJJ_FacturaDeta mEJJ_FacturaDeta)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_FacturaDeta.Add(mEJJ_FacturaDeta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdArticulo = new SelectList(db.MEJJ_Articulo, "IdArticulo", "Descripcion", mEJJ_FacturaDeta.IdArticulo);
            ViewBag.IdFacturaE = new SelectList(db.MEJJ_FacturaEnca, "IdFacturaE", "Tipo", mEJJ_FacturaDeta.IdFacturaE);
            return View(mEJJ_FacturaDeta);
        }

        // GET: FacturaDeta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaDeta mEJJ_FacturaDeta = db.MEJJ_FacturaDeta.Find(id);
            if (mEJJ_FacturaDeta == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArticulo = new SelectList(db.MEJJ_Articulo, "IdArticulo", "Descripcion", mEJJ_FacturaDeta.IdArticulo);
            ViewBag.IdFacturaE = new SelectList(db.MEJJ_FacturaEnca, "IdFacturaE", "Tipo", mEJJ_FacturaDeta.IdFacturaE);
            return View(mEJJ_FacturaDeta);
        }

        // POST: FacturaDeta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFacturaD,IdFacturaE,IdArticulo,Cantidad,PorcentajeDesc,SubTotal,Descuento,Impuesto")] MEJJ_FacturaDeta mEJJ_FacturaDeta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_FacturaDeta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdArticulo = new SelectList(db.MEJJ_Articulo, "IdArticulo", "Descripcion", mEJJ_FacturaDeta.IdArticulo);
            ViewBag.IdFacturaE = new SelectList(db.MEJJ_FacturaEnca, "IdFacturaE", "Tipo", mEJJ_FacturaDeta.IdFacturaE);
            return View(mEJJ_FacturaDeta);
        }

        // GET: FacturaDeta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaDeta mEJJ_FacturaDeta = db.MEJJ_FacturaDeta.Find(id);
            if (mEJJ_FacturaDeta == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_FacturaDeta);
        }

        // POST: FacturaDeta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_FacturaDeta mEJJ_FacturaDeta = db.MEJJ_FacturaDeta.Find(id);
            db.MEJJ_FacturaDeta.Remove(mEJJ_FacturaDeta);
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
