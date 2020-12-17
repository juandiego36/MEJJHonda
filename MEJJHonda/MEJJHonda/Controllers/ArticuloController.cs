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
using System.Data.Entity.Validation;
using System.Data.SqlClient;

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
            try
            {
                MEJJ_Articulo mEJJ_Articulo = db.MEJJ_Articulo.Find(id);
                db.MEJJ_Articulo.Remove(mEJJ_Articulo);
                db.SaveChanges();
                TempData["type"] = "success";
                TempData["message"] = "Se elimino exitosamente";
            }
            catch (Exception ex)
            {
                TempData["type"] = "error";
                TempData["message"] = "Este valor no se puede eliminar";
            }

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
			DataTable dtArticulos = getArticulosToExport(id);
			//DataTable dtEncabezado = getEncabezado();
			//DataTable dtVacio1 = getvacio(1);
			//DataTable dtVacio2 = getvacio(2);
			//DataTable dtPie = getPie();
			using (XLWorkbook woekBook = new XLWorkbook())
			{
				//woekBook.Worksheets.Add(dtEncabezado);
				//woekBook.Worksheets.Add(dtVacio1);
				woekBook.Worksheets.Add(dtArticulos);
				//woekBook.Worksheets.Add(dtVacio2);
				//woekBook.Worksheets.Add(dtPie);
				using (MemoryStream stream = new MemoryStream())
				{
					woekBook.SaveAs(stream);
					return File(
						stream.ToArray(), 
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
						"Articulos.xlsx"
					);
				}
			}
		}

		public ActionResult ExportToPdf(int? id)
		{
			DataTable dtArticulos = getArticulosToExport(id);
			if (dtArticulos.Rows.Count > 0)
			{
				int pdfRowIndex = 1;
				string filename = "Articulos-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
				string filepath = Server.MapPath("\\") + "" + filename + ".pdf";
				Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
				FileStream fs = new FileStream(filepath, FileMode.Create);
				PdfWriter writer = PdfWriter.GetInstance(document, fs);
				document.Open();
				Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
				Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);
				float[] columnDefinitionSize = { 5F, 2F, 2F, 2F, 2F };
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
				table.AddCell(new Phrase("Descripcion", font1));
				table.AddCell(new Phrase("Modelo", font1));
				table.AddCell(new Phrase("Precio", font1));
				table.AddCell(new Phrase("Color", font1));
				table.AddCell(new Phrase("Tamanio", font1));
				table.HeaderRows = 1;
				foreach (DataRow data in dtArticulos.Rows)
				{
					table.AddCell(new Phrase(data["Descripcion"].ToString(), font2));
					table.AddCell(new Phrase(data["Modelo"].ToString(), font2));
					table.AddCell(new Phrase(data["Precio"].ToString(), font2));
					table.AddCell(new Phrase(data["Color"].ToString(), font2));
					table.AddCell(new Phrase(data["Tamanio"].ToString(), font2));
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
			DataTable dtArticulos = getArticulosToExport(id);
			if (dtArticulos.Rows.Count > 0)
			{
				StringBuilder sbDocumentBody = new StringBuilder();
				sbDocumentBody.Append(
					String.Concat(
						"Solicitante: ", 
						Session["Nombre"] != null ? Session["Nombre"].ToString() : "")
				);
				sbDocumentBody.Append("Fecha: " + DateTime.Now.ToString());
				sbDocumentBody.Append("<table width=\"100%\" style=\"background-color:#ffffff;\">");
				if (dtArticulos.Rows.Count > 0)
				{
					sbDocumentBody.Append("<tr><td>");
					sbDocumentBody.Append("<table width=\"600\" cellpadding=0 cellspacing=0 style=\"border: 1px solid gray;\">");
					// Add Column Headers dynamically from datatable  
					sbDocumentBody.Append("<tr>");
					for (int i = 0; i < dtArticulos.Columns.Count; i++)
					{
						sbDocumentBody.Append("<td class=\"Header\" width=\"120\" style=\"border: 1px solid gray; text-align:center; font-family:Verdana; font-size:12px; font-weight:bold;\">" + dtArticulos.Columns[i].ToString().Replace(".", "<br>") + "</td>");
					}
					sbDocumentBody.Append("</tr>");
					// Add Data Rows dynamically from datatable  
					for (int i = 0; i < dtArticulos.Rows.Count; i++)
					{
						sbDocumentBody.Append("<tr>");
						for (int j = 0; j < dtArticulos.Columns.Count; j++)
						{
							sbDocumentBody.Append("<td class=\"Content\"style=\"border: 1px solid gray;\">" + dtArticulos.Rows[i][j].ToString() + "</td>");
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
				Response.AppendHeader("Content-disposition", "attachment; filename=Articulos.doc");
				Response.Write(sbDocumentBody.ToString());
				Response.End();
			}
			return RedirectToAction("Index");
		}

		private DataTable getArticulosToExport(int? id)
		{
			var articulos = new List<MEJJ_Articulo>();
			if (id != null)
			{
				articulos.Add(db.MEJJ_Articulo.Find(id));
			} else
			{
				articulos = db.MEJJ_Articulo.ToList();
			}
			DataTable dtArticulos = new DataTable("Articulos");
			dtArticulos.Columns.AddRange(
				new DataColumn[5] {
					new DataColumn("Descripcion"),
					new DataColumn("Modelo"),
					new DataColumn("Precio"),
					new DataColumn("Color"),
					new DataColumn("Tamanio")
				}
			);
			foreach (var articulo in articulos)
			{
				dtArticulos.Rows.Add(
					articulo.Descripcion, 
					articulo.Modelo, 
					articulo.Precio, 
					articulo.Color,
					articulo.Tamanio
				);
			}
			return dtArticulos;
		}

		private DataTable getEncabezado()
		{
			DataTable dtEncabezado = new DataTable("Encabezado");
			dtEncabezado.Columns.AddRange(
				new DataColumn[2] {
					new DataColumn("Solicitante"),
					new DataColumn("Fecha")
				}
			);
			dtEncabezado.Rows.Add(
				Session["Nombre"] != null ? Session["Nombre"].ToString() : "",
				DateTime.Now.ToString()
			);
			return dtEncabezado;
		}

		private DataTable getvacio(int numero)
		{
			DataTable dtVacio = new DataTable("Vacio" + numero);
			dtVacio.Columns.AddRange(
				new DataColumn[1] {
					new DataColumn("")
				}
			);
			dtVacio.Rows.Add("");
			return dtVacio;
		}
		private DataTable getPie()
		{
			DataTable dtPie = new DataTable("Pie");
			dtPie.Columns.AddRange(
				new DataColumn[1] {
					new DataColumn("Empresa")
				}
			);
			dtPie.Rows.Add("MEJJ");
			return dtPie;
		}
	}
}
