using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MejjHonda.Models;

namespace MejjHonda.Controllers
{
    public class FacturaEncaController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: FacturaEnca
        public ActionResult Index()
        {
            var mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Include(m => m.MEJJ_Cliente).Include(m => m.MEJJ_Empleado).Include(m => m.MEJJ_Moneda);
            return View(mEJJ_FacturaEnca.ToList());
        }

        // GET: FacturaEnca/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaEnca mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Find(id);
            if (mEJJ_FacturaEnca == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_FacturaEnca);
        }

        // GET: FacturaEnca/Create
        public ActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(db.MEJJ_Cliente, "IdCliente", "Nombre");
            ViewBag.IdEmpleado = new SelectList(db.MEJJ_Empleado, "IdEmpleado", "Nombre");
            ViewBag.IdMoneda = new SelectList(db.MEJJ_Moneda, "Codigo", "Nombre");
            ViewBag.Articulos = db.MEJJ_Articulo.ToList();
            return View();
        }

        // POST: FacturaEnca/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFacturaE,IdEmpleado,IdCliente,Tipo,Fecha,FechaIng,Observacion,MedioPago,TipoCambio,Subtotal,Descuento,Impuesto,IdMoneda")] MEJJ_FacturaEnca mEJJ_FacturaEnca)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_FacturaEnca.Add(mEJJ_FacturaEnca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCliente = new SelectList(db.MEJJ_Cliente, "IdCliente", "Cedula", mEJJ_FacturaEnca.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.MEJJ_Empleado, "IdEmpleado", "Nombre", mEJJ_FacturaEnca.IdEmpleado);
            ViewBag.IdMoneda = new SelectList(db.MEJJ_Moneda, "IdMoneda", "Codigo", mEJJ_FacturaEnca.IdMoneda);
            return View(mEJJ_FacturaEnca);
        }


        // GET: FacturaEnca/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaEnca mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Find(id);
            if (mEJJ_FacturaEnca == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.MEJJ_Cliente, "IdCliente", "Cedula", mEJJ_FacturaEnca.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.MEJJ_Empleado, "IdEmpleado", "Nombre", mEJJ_FacturaEnca.IdEmpleado);
            ViewBag.IdMoneda = new SelectList(db.MEJJ_Moneda, "IdMoneda", "Codigo", mEJJ_FacturaEnca.IdMoneda);
            return View(mEJJ_FacturaEnca);
        }

        // POST: FacturaEnca/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFacturaE,IdEmpleado,IdCliente,Tipo,Fecha,FechaIng,Observacion,MedioPago,TipoCambio,Subtotal,Descuento,Impuesto,IdMoneda")] MEJJ_FacturaEnca mEJJ_FacturaEnca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_FacturaEnca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.MEJJ_Cliente, "IdCliente", "Cedula", mEJJ_FacturaEnca.IdCliente);
            ViewBag.IdEmpleado = new SelectList(db.MEJJ_Empleado, "IdEmpleado", "Nombre", mEJJ_FacturaEnca.IdEmpleado);
            ViewBag.IdMoneda = new SelectList(db.MEJJ_Moneda, "IdMoneda", "Codigo", mEJJ_FacturaEnca.IdMoneda);
            return View(mEJJ_FacturaEnca);
        }

        // GET: FacturaEnca/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_FacturaEnca mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Find(id);
            if (mEJJ_FacturaEnca == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_FacturaEnca);
        }

        // POST: FacturaEnca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_FacturaEnca mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Find(id);
            db.MEJJ_FacturaEnca.Remove(mEJJ_FacturaEnca);
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
