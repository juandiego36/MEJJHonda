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
                    UserInfo userInfo = new UserInfo();

                    userInfo.Mail = empleadoModel.Mail;
                    userInfo.Nombre = empleadoModel.Nombre;
                    userInfo.IdEmpleado = empleadoDetails.IdEmpleado;

                    Session["userInfo"] = userInfo;
                    return RedirectToAction("Index","Home");

                    ; }
            }
            return View();
        }
    }

    public class UserInfo{
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
    }
}