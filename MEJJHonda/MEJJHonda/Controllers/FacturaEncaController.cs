using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MejjHonda.Models;


namespace MejjHonda.Controllers{

    public class FacturaEncaController : Controller{

        private MejjHondaEntities db = new MejjHondaEntities();
        
        public ActionResult Index(){
            var mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Include(m => m.MEJJ_Cliente).Include(m => m.MEJJ_Empleado).Include(m => m.MEJJ_Moneda);
            return View(mEJJ_FacturaEnca.ToList());
        }
      
        public ActionResult Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<MEJJ_FacturaDeta> mEJJ_FacturaDeta = new List<MEJJ_FacturaDeta>();
            MEJJ_FacturaEnca mEJJ_FacturaEnca = db.MEJJ_FacturaEnca.Find(id);
            mEJJ_FacturaDeta = (from a in db.MEJJ_FacturaDeta where a.IdFacturaE == id select a).ToList();
            mEJJ_FacturaEnca.MEJJ_FacturaDeta = mEJJ_FacturaDeta;
            ViewBag.total = Convert.ToDecimal(mEJJ_FacturaEnca.Subtotal) + Convert.ToDecimal(mEJJ_FacturaEnca.Impuesto);
            if (mEJJ_FacturaEnca == null){
                return HttpNotFound();
            }
            return View(mEJJ_FacturaEnca);
        }
        
        public ActionResult Create(){
            ViewBag.Clientes = db.MEJJ_Cliente.ToList();
            ViewBag.IdEmpleado = new SelectList(db.MEJJ_Empleado, "IdEmpleado", "Nombre");
            ViewBag.IdMoneda = new SelectList(db.MEJJ_Moneda, "Codigo", "Nombre");
            ViewBag.Articulos = db.MEJJ_Articulo.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GuardarFactura(MEJJ_FacturaEnca mEJJ_FacturaEnca, List<MEJJ_FacturaDeta> mEJJ_FacturaDeta){

            mEJJ_FacturaEnca.IdEmpleado = Convert.ToInt32(Session["IdEmpleado"]);
            mEJJ_FacturaEnca.MEJJ_FacturaDeta = mEJJ_FacturaDeta;
            db.MEJJ_FacturaEnca.Add(mEJJ_FacturaEnca);
            var resultado = db.SaveChanges();
            if (resultado > 0)
            {
                TempData["type"] = "success";
                TempData["message"] = "La factura se guardó correctamente";
                return View();
        }
            else{
                TempData["type"] = "error";
                TempData["message"] = "No se pudo guardar la factura, inténte nuevamente";
                return View(mEJJ_FacturaEnca);
    }

}

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
