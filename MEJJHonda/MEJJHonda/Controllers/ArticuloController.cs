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
    public class ArticuloController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: MEJJ_Articulo
        public ActionResult Index(String busqueda)
        {
			return View(
				!String.IsNullOrEmpty(busqueda) ? 
					db.MEJJ_Articulo.Where(articulo => articulo.Descripcion.Contains(busqueda)).ToList()
				: db.MEJJ_Articulo.ToList()
			);
        }

        // GET: MEJJ_Articulo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Articulo mEJJ_Articulo = db.MEJJ_Articulo.Find(id);
            if (mEJJ_Articulo == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Articulo);
        }

        // GET: MEJJ_Articulo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MEJJ_Articulo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdArticulo,Descripcion,Modelo,Precio,Color,Tamanio")] MEJJ_Articulo mEJJ_Articulo)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_Articulo.Add(mEJJ_Articulo);
                db.SaveChanges();
                TempData["type"] = "success";
                TempData["message"] = "Se creo exitosamente";
                return RedirectToAction("Index");
            }

            return View(mEJJ_Articulo);
        }

        // GET: MEJJ_Articulo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Articulo mEJJ_Articulo = db.MEJJ_Articulo.Find(id);
            if (mEJJ_Articulo == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Articulo);
        }

        // POST: MEJJ_Articulo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdArticulo,Descripcion,Modelo,Precio,Color,Tamanio")] MEJJ_Articulo mEJJ_Articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_Articulo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["type"] = "success";
                TempData["message"] = "Se edito exitosamente";
                return RedirectToAction("Index");
            }
            return View(mEJJ_Articulo);
        }

        // GET: MEJJ_Articulo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Articulo mEJJ_Articulo = db.MEJJ_Articulo.Find(id);
            if (mEJJ_Articulo == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Articulo);
        }

        // POST: MEJJ_Articulo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_Articulo mEJJ_Articulo = db.MEJJ_Articulo.Find(id);
            db.MEJJ_Articulo.Remove(mEJJ_Articulo);
            db.SaveChanges();
            TempData["type"] = "success";
            TempData["message"] = "Se elimino exitosamente";
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
