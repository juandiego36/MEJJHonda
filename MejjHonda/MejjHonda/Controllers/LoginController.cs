using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MejjHonda.Models;

namespace MejjHonda.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(MejjHonda.Models.MEJJ_Empleado empleadoModel)
        {
            using (MejjHondaEntities db = new MejjHondaEntities()) {
                var empleadoDetails = db.MEJJ_Empleado.Where(x => x.Mail == empleadoModel.Mail && x.Contraseña == empleadoModel.Contraseña).FirstOrDefault();
                if (empleadoDetails == null)
                {
                    empleadoModel.LoginErrorMessage = "Contrasena o correo incorrectos";
                    return View("Index", empleadoModel);
                }
                else {


                    Session["IdEmpleado"] = empleadoDetails.IdEmpleado;
                    Session["Nombre"] = empleadoDetails.Nombre;
           

                    return RedirectToAction("Index","Home");
                }
            }
        }

        public ActionResult LogOut() {
            Session.Abandon();
            return RedirectToAction("Index");

        }
    }

}