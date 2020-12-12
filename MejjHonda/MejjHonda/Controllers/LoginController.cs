using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MejjHonda.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using System.Web.Helpers;


namespace MejjHonda.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (Session["IdEmpleado"] == null)
            {
                Session.Abandon();
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(MejjHonda.Models.MEJJ_Empleado empleadoModel)
        {
            using (MejjHondaEntities db = new MejjHondaEntities()) {
                
                var empleadoDetails = db.MEJJ_Empleado.Where(x => x.Mail == empleadoModel.Mail).FirstOrDefault();
                if (empleadoDetails == null) {
                    TempData["type"] = "error";
                    TempData["message"] = "Contraseña o correo incorrectos";
                    return View("Index", empleadoModel);
                }
                if (string.Compare(Crypto.Hash(empleadoModel.Contraseña), empleadoDetails.Contraseña) == 0)
                {
                    Session["IdEmpleado"] = empleadoDetails.IdEmpleado;
                    Session["Nombre"] = empleadoDetails.Nombre;


                    return RedirectToAction("Index", "Home");

                }else{
                    TempData["type"] = "error";
                    TempData["message"] = "Contraseña o correo incorrectos";
                    return View("Index", empleadoModel);

                }
            }
        }

        public ActionResult LogOut() {
            Session.Abandon();
            return RedirectToAction("Index");

        }


        public ActionResult ForgotPassword()
        {
            Session.Abandon();
            return View();

        }
        [HttpPost]
        public ActionResult ForgotPassword(string email) {
            string message = "";

            using (MejjHondaEntities db = new MejjHondaEntities())
            {
                
                var empleado = db.MEJJ_Empleado.Where(x => x.Mail == email).FirstOrDefault();
                if (empleado != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    empleado.Codigo_Contrasena = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    TempData["type"] = "success";
                    TempData["message"] = "Se envio el correo a su cuenta";
                    SendVerificationEmail(email, resetCode);
                    Session["Codigo"] = resetCode;

                }
                else {
                    TempData["type"] = "error";
                    TempData["message"] = "La cuenta no fue encontrada";
                }
            }
                ViewBag.Message = message;
                return View();
        }

        [NonAction]
        public void SendVerificationEmail(string email, string activationCode)
        {
            var verifyUrl = "/Login/ResetPassword/"+activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("mejj.notreply@gmail.com", "MEJJ");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "ContrasenaUltraSecreta37"; // Replace with actual password
            string subject = "Haga clic en el botón para cambiar su contraseña!";

            string body = "<br/><br/>" +
                " Haz clic en el link de abajo para cambiar tu contraseña:" +
                " <br/><br/><a href='" + link + "'> Cambia tu contraseña </a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            
            smtp.Send(message);
        }

        public ActionResult ResetPassword() {
            using (MejjHondaEntities db = new MejjHondaEntities())//
            {
                if (Session["Codigo"] != null)
                {
                    string codigo = Session["Codigo"].ToString();

                    var empleado = db.MEJJ_Empleado.Where(x => x.Codigo_Contrasena == codigo).FirstOrDefault();
                    if (empleado != null)
                    {
                        Session.Abandon();
                        ResetPasswordModel model = new ResetPasswordModel();
                        model.ResetCode = codigo;
                        return View(model);
                    }
                }      
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            using (MejjHondaEntities dc = new MejjHondaEntities())
            {
                var user = dc.MEJJ_Empleado.Where(a => a.Codigo_Contrasena == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Contraseña = Crypto.Hash(model.NewPassword);
                    user.Codigo_Contrasena = "";
                    dc.SaveChanges();
                    TempData["type"] = "success";
                    TempData["message"] = "Su contraseña se cambio exitosamente";
                }
            }

            return RedirectToAction("Index");
        }


    }

}