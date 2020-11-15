﻿using System;
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
    public class EmpleadoController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: Empleado
        public ActionResult Index()
        {
            return View(db.MEJJ_Empleado.ToList());
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Empleado mEJJ_Empleado = db.MEJJ_Empleado.Find(id);
            if (mEJJ_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmpleado,Nombre,Cedula,Mail,Telefono,Direccion,IdRol,Contraseña")] MEJJ_Empleado mEJJ_Empleado)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_Empleado.Add(mEJJ_Empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mEJJ_Empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Empleado mEJJ_Empleado = db.MEJJ_Empleado.Find(id);
            if (mEJJ_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEmpleado,Nombre,Cedula,Mail,Telefono,Direccion,IdRol,Contraseña")] MEJJ_Empleado mEJJ_Empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_Empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mEJJ_Empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Empleado mEJJ_Empleado = db.MEJJ_Empleado.Find(id);
            if (mEJJ_Empleado == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_Empleado mEJJ_Empleado = db.MEJJ_Empleado.Find(id);
            db.MEJJ_Empleado.Remove(mEJJ_Empleado);
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
