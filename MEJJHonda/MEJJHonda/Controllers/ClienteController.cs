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
    public class ClienteController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: Cliente
        public ActionResult Index()
        {
            return View(db.MEJJ_Cliente.ToList());
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Cliente mEJJ_Cliente = db.MEJJ_Cliente.Find(id);
            if (mEJJ_Cliente == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Cedula,Telefono,Direccion,DiasCredito,Nombre,Mail")] MEJJ_Cliente mEJJ_Cliente)
        {
            if (ModelState.IsValid)
            {
                db.MEJJ_Cliente.Add(mEJJ_Cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mEJJ_Cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Cliente mEJJ_Cliente = db.MEJJ_Cliente.Find(id);
            if (mEJJ_Cliente == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Cedula,Telefono,Direccion,DiasCredito,Nombre,Mail")] MEJJ_Cliente mEJJ_Cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mEJJ_Cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mEJJ_Cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MEJJ_Cliente mEJJ_Cliente = db.MEJJ_Cliente.Find(id);
            if (mEJJ_Cliente == null)
            {
                return HttpNotFound();
            }
            return View(mEJJ_Cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MEJJ_Cliente mEJJ_Cliente = db.MEJJ_Cliente.Find(id);
            db.MEJJ_Cliente.Remove(mEJJ_Cliente);
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