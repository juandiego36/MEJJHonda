using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MejjHonda.Models;

namespace MejjHonda.Controllers
{
    public class ClienteController : Controller
    {
        private MejjHondaEntities db = new MejjHondaEntities();

        // GET: Cliente
        public ActionResult Index(String busqueda)
        {
            return View(
				!String.IsNullOrEmpty(busqueda) ? 
					db.MEJJ_Cliente.Where(cliente => cliente.Nombre.Contains(busqueda)).ToList()
				: db.MEJJ_Cliente.ToList()
			);
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
                TempData["type"] = "success";
                TempData["message"] = "Se creo exitosamente";
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
                TempData["type"] = "success";
                TempData["message"] = "Se edito exitosamente";
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
			DataTable dtClientes = getClientesToExport(id);
			using (XLWorkbook woekBook = new XLWorkbook())
			{
				woekBook.Worksheets.Add(dtClientes);
				using (MemoryStream stream = new MemoryStream())
				{
					woekBook.SaveAs(stream);
					return File(
						stream.ToArray(),
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Clientes.xlsx"
					);
				}
			}
		}

		public ActionResult ExportToPdf(int? id)
		{
			DataTable dtClientes = getClientesToExport(id);
			if (dtClientes.Rows.Count > 0)
			{
				int pdfRowIndex = 1;
				string filename = "Clientes-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
				string filepath = Server.MapPath("\\") + "" + filename + ".pdf";
				Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
				FileStream fs = new FileStream(filepath, FileMode.Create);
				PdfWriter writer = PdfWriter.GetInstance(document, fs);
				document.Open();
				Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
				Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);
				float[] columnDefinitionSize = { 2F, 2F, 5F, 2F, 2F, 2F };
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
				table.AddCell(new Phrase("Cedula", font1));
				table.AddCell(new Phrase("Telefono", font1));
				table.AddCell(new Phrase("Direccion", font1));
				table.AddCell(new Phrase("Dias Credito", font1));
				table.AddCell(new Phrase("Nombre", font1));
				table.AddCell(new Phrase("Correo", font1));
				table.HeaderRows = 1;
				foreach (DataRow data in dtClientes.Rows)
				{
					table.AddCell(new Phrase(data["Cedula"].ToString(), font2));
					table.AddCell(new Phrase(data["Telefono"].ToString(), font2));
					table.AddCell(new Phrase(data["Direccion"].ToString(), font2));
					table.AddCell(new Phrase(data["DiasCredito"].ToString(), font2));
					table.AddCell(new Phrase(data["Nombre"].ToString(), font2));
					table.AddCell(new Phrase(data["Mail"].ToString(), font2));
					pdfRowIndex++;
				}
				document.Add(
					new Phrase(
						String.Concat("Solicitante: ", Session["Nombre"] != null ? Session["Nombre"].ToString() : ""),
						font2
					)
				);
				document.Add(
					new Phrase(
						"Fecha: " + DateTime.Now.ToString(),
						font2
					)
				);
				document.Add(table);
				document.Add(
					new Phrase(
						"Empresa: MEJJ",
						font2
					)
				);
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
			DataTable dtClientes = getClientesToExport(id);
			if (dtClientes.Rows.Count > 0)
			{
				StringBuilder sbDocumentBody = new StringBuilder();
				sbDocumentBody.Append(
					String.Concat(
						"Solicitante: ",
						Session["Nombre"] != null ? Session["Nombre"].ToString() : "")
				);
				sbDocumentBody.Append("Fecha: " + DateTime.Now.ToString());
				sbDocumentBody.Append("<table width=\"100%\" style=\"background-color:#ffffff;\">");
				if (dtClientes.Rows.Count > 0)
				{
					sbDocumentBody.Append("<tr><td>");
					sbDocumentBody.Append("<table width=\"600\" cellpadding=0 cellspacing=0 style=\"border: 1px solid gray;\">");
					// Add Column Headers dynamically from datatable  
					sbDocumentBody.Append("<tr>");
					for (int i = 0; i < dtClientes.Columns.Count; i++)
					{
						sbDocumentBody.Append("<td class=\"Header\" width=\"120\" style=\"border: 1px solid gray; text-align:center; font-family:Verdana; font-size:12px; font-weight:bold;\">" + dtClientes.Columns[i].ToString().Replace(".", "<br>") + "</td>");
					}
					sbDocumentBody.Append("</tr>");
					// Add Data Rows dynamically from datatable  
					for (int i = 0; i < dtClientes.Rows.Count; i++)
					{
						sbDocumentBody.Append("<tr>");
						for (int j = 0; j < dtClientes.Columns.Count; j++)
						{
							sbDocumentBody.Append("<td class=\"Content\"style=\"border: 1px solid gray;\">" + dtClientes.Rows[i][j].ToString() + "</td>");
						}
						sbDocumentBody.Append("</tr>");
					}
					sbDocumentBody.Append("</table>");
					sbDocumentBody.Append("</td></tr></table>");
				}
				sbDocumentBody.Append("Empresa: MEJJ");
				Response.Clear();
				Response.Buffer = true;
				Response.AppendHeader("Content-Type", "application/msword");
				Response.AppendHeader("Content-disposition", "attachment; filename=Clientes.doc");
				Response.Write(sbDocumentBody.ToString());
				Response.End();
			}
			return RedirectToAction("Index");
		}

		private DataTable getClientesToExport(int? id)
		{
			var clientes = new List<MEJJ_Cliente>();
			if (id != null)
			{
				clientes.Add(db.MEJJ_Cliente.Find(id));
			}
			else
			{
				clientes = db.MEJJ_Cliente.ToList();
			}
			DataTable dtClientes = new DataTable("Clientes");
			dtClientes.Columns.AddRange(
				new DataColumn[6] {
					new DataColumn("Cedula"),
					new DataColumn("Telefono"),
					new DataColumn("Direccion"),
					new DataColumn("DiasCredito"),
					new DataColumn("Nombre"),
					new DataColumn("Mail")
				}
			);
			foreach (var cliente in clientes)
			{
				dtClientes.Rows.Add(
					cliente.Cedula,
					cliente.Telefono,
					cliente.Direccion,
					cliente.DiasCredito,
					cliente.Nombre,
					cliente.Mail
				);
			}
			return dtClientes;
		}
	}
}
