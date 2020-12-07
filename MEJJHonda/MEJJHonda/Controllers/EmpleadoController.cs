using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MejjHonda.Models;
using System.Web.Helpers;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace MejjHonda.Controllers
{
    public class EmpleadoController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: Empleado
        public ActionResult Index(String busqueda)
        {
            return View(
				!String.IsNullOrEmpty(busqueda) ?
					db.MEJJ_Empleado.Where(empleado => empleado.Nombre.Contains(busqueda)).ToList()
				: db.MEJJ_Empleado.ToList()
			);
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
                var correoUnico = db.MEJJ_Empleado.Where(x => x.Mail == mEJJ_Empleado.Mail).FirstOrDefault();
                if (correoUnico != null)
                {
                    mEJJ_Empleado.LoginErrorMessage = "El correo ya esta en uso";
                    return View(mEJJ_Empleado);
                }
                else
                {
                    mEJJ_Empleado.Contraseña = Crypto.Hash(mEJJ_Empleado.Contraseña);
                    db.MEJJ_Empleado.Add(mEJJ_Empleado);
                    db.SaveChanges();
                    TempData["type"] = "success";
                    TempData["message"] = "Se creo exitosamente";
                    return RedirectToAction("Index");
                }

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
                var correoUnico = db.MEJJ_Empleado.Where(x => x.Mail == mEJJ_Empleado.Mail && x.IdEmpleado != mEJJ_Empleado.IdEmpleado).FirstOrDefault();
                if (correoUnico != null)
                {
                    mEJJ_Empleado.LoginErrorMessage = "El correo ya esta en uso";
                    return View(mEJJ_Empleado);
                }
                else
                {
                    mEJJ_Empleado.Contraseña = Crypto.Hash(mEJJ_Empleado.Contraseña);
                    db.Entry(mEJJ_Empleado).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["type"] = "success";
                    TempData["message"] = "Se edito exitosamente";
                    return RedirectToAction("Index");
                }

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

		public ActionResult ExportToExcel(int? id)
		{
			DataTable dtEmpleados = getEmpleadosToExport(id);
			using (XLWorkbook woekBook = new XLWorkbook())
			{
				woekBook.Worksheets.Add(dtEmpleados);
				using (MemoryStream stream = new MemoryStream())
				{
					woekBook.SaveAs(stream);
					return File(
						stream.ToArray(),
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Colaboradores.xlsx"
					);
				}
			}
		}

		public ActionResult ExportToPdf(int? id)
		{
			DataTable dtEmpleados = getEmpleadosToExport(id);
			if (dtEmpleados.Rows.Count > 0)
			{
				int pdfRowIndex = 1;
				string filename = "Colaboradores-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
				string filepath = Server.MapPath("\\") + "" + filename + ".pdf";
				Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
				FileStream fs = new FileStream(filepath, FileMode.Create);
				PdfWriter writer = PdfWriter.GetInstance(document, fs);
				document.Open();
				Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
				Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);
				float[] columnDefinitionSize = { 2F, 2F, 2F, 2F, 5F };
				PdfPTable table;
				PdfPCell cell;
				table = new PdfPTable(columnDefinitionSize)
				{
					WidthPercentage = 100
				};
				cell = new PdfPCell
				{
					BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
				};
				table.AddCell(new Phrase("Nombre", font1));
				table.AddCell(new Phrase("Cedula", font1));
				table.AddCell(new Phrase("Correo", font1));
				table.AddCell(new Phrase("Telefono", font1));
				table.AddCell(new Phrase("Direccion", font1));
				table.HeaderRows = 1;
				foreach (DataRow data in dtEmpleados.Rows)
				{
					table.AddCell(new Phrase(data["Nombre"].ToString(), font2));
					table.AddCell(new Phrase(data["Cedula"].ToString(), font2));
					table.AddCell(new Phrase(data["Mail"].ToString(), font2));
					table.AddCell(new Phrase(data["Telefono"].ToString(), font2));
					table.AddCell(new Phrase(data["Direccion"].ToString(), font2));
					pdfRowIndex++;
				}
				document.Add(table);
				document.Close();
				document.CloseDocument();
				document.Dispose();
				writer.Close();
				writer.Dispose();
				fs.Close();
				fs.Dispose();
				FileStream sourceFile = new FileStream(filepath, FileMode.Open);
				float fileSize = 0;
				fileSize = sourceFile.Length;
				byte[] getContent = new byte[Convert.ToInt32(Math.Truncate(fileSize))];
				sourceFile.Read(getContent, 0, Convert.ToInt32(sourceFile.Length));
				sourceFile.Close();
				Response.ClearContent();
				Response.ClearHeaders();
				Response.Buffer = true;
				Response.ContentType = "application/pdf";
				Response.AddHeader("Content-Length", getContent.Length.ToString());
				Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".pdf;");
				Response.BinaryWrite(getContent);
				Response.Flush();
				Response.End();
			}
			return RedirectToAction("Index");
		}

		public ActionResult ExportToWord(int? id)
		{
			DataTable dtEmpleados = getEmpleadosToExport(id);
			if (dtEmpleados.Rows.Count > 0)
			{
				StringBuilder sbDocumentBody = new StringBuilder();
				sbDocumentBody.Append("<table width=\"100%\" style=\"background-color:#ffffff;\">");
				if (dtEmpleados.Rows.Count > 0)
				{
					sbDocumentBody.Append("<tr><td>");
					sbDocumentBody.Append("<table width=\"600\" cellpadding=0 cellspacing=0 style=\"border: 1px solid gray;\">");
					// Add Column Headers dynamically from datatable  
					sbDocumentBody.Append("<tr>");
					for (int i = 0; i < dtEmpleados.Columns.Count; i++)
					{
						sbDocumentBody.Append("<td class=\"Header\" width=\"120\" style=\"border: 1px solid gray; text-align:center; font-family:Verdana; font-size:12px; font-weight:bold;\">" + dtEmpleados.Columns[i].ToString().Replace(".", "<br>") + "</td>");
					}
					sbDocumentBody.Append("</tr>");
					// Add Data Rows dynamically from datatable  
					for (int i = 0; i < dtEmpleados.Rows.Count; i++)
					{
						sbDocumentBody.Append("<tr>");
						for (int j = 0; j < dtEmpleados.Columns.Count; j++)
						{
							sbDocumentBody.Append("<td class=\"Content\"style=\"border: 1px solid gray;\">" + dtEmpleados.Rows[i][j].ToString() + "</td>");
						}
						sbDocumentBody.Append("</tr>");
					}
					sbDocumentBody.Append("</table>");
					sbDocumentBody.Append("</td></tr></table>");
				}
				Response.Clear();
				Response.Buffer = true;
				Response.AppendHeader("Content-Type", "application/msword");
				Response.AppendHeader("Content-disposition", "attachment; filename=Colaboradores.doc");
				Response.Write(sbDocumentBody.ToString());
				Response.End();
			}
			return RedirectToAction("Index");
		}

		private DataTable getEmpleadosToExport(int? id)
		{
			var empleados = new List<MEJJ_Empleado>();
			if (id != null)
			{
				empleados.Add(db.MEJJ_Empleado.Find(id));
			}
			else
			{
				empleados = db.MEJJ_Empleado.ToList();
			}
			DataTable dtEmpleados = new DataTable("Empleados");
			dtEmpleados.Columns.AddRange(
				new DataColumn[5] {
					new DataColumn("Nombre"),
					new DataColumn("Cedula"),
					new DataColumn("Mail"),
					new DataColumn("Telefono"),
					new DataColumn("Direccion")
				}
			);
			foreach (var empleado in empleados)
			{
				dtEmpleados.Rows.Add(
					empleado.Nombre,
					empleado.Cedula,
					empleado.Mail,
					empleado.Telefono,
					empleado.Direccion
				);
			}
			return dtEmpleados;
		}
	}
}
