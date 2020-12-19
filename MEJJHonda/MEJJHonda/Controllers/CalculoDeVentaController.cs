using iTextSharp.text;
using iTextSharp.text.pdf;
using MejjHonda.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MejjHonda.Controllers
{
    public class CalculoDeVentaController : Controller
    {
		public static List<Models.Banco> bancos;

        // GET: CalculoDeVenta
        public ActionResult Index(double precio)
        {
			bancos = new List<Models.Banco>();
			double tarifaFijaBac = 6.7;
			double tarifaFijaNacional = 8;
			double tarifaFijaScotiabank = 6.9;
			double prima = Math.Round(10 * precio / 100);
			List<int> plazos = new List<int>();
			plazos.Add(12);
			plazos.Add(18);
			plazos.Add(24);
			int plazo = plazos[0];
			List<string> descripcionesBac = new List<string>();
			descripcionesBac.Add(
				"* Cuota corresponde a cálculo realizado con tasa fija de BAC aplicable para los primeros 24 meses."
			);
			descripcionesBac.Add("* Cuota sin seguro");
			descripcionesBac.Add("Revisar condiciones en: http://ejemplo.com/reglamentos");
			List<string> descripcionesNacional = new List<string>();
			descripcionesNacional.Add(
				"* Cuota corresponde a cálculo realizado con tasa fija de BNCR aplicable para los primeros 24 meses."
			);
			descripcionesNacional.Add("* Cuota sin seguro");
			descripcionesNacional.Add("Revisar condiciones en: http://ejemplo.com/reglamentos");
			List<string> descripcionesScotiabank = new List<string>();
			descripcionesScotiabank.Add(
				"* Cuota corresponde a cálculo realizado con tasa fija de Scotiabank aplicable para los primeros 24 meses."
			);
			descripcionesScotiabank.Add("* Cuota sin seguro");
			descripcionesScotiabank.Add("Revisar condiciones en: http://ejemplo.com/reglamentos");
			bancos.Add(
				new Models.Banco(
					"BacLogo.png", 
					tarifaFijaBac, 
					prima, 
					plazos,
					plazo,
					descripcionesBac, 
					this.calcularTarifa(tarifaFijaBac, plazo, precio, prima),
					"bac",
					precio
				)
			);
			bancos.Add(
				new Models.Banco(
					"NacionalLogo.png",
					tarifaFijaNacional, 
					prima, 
					plazos, 
					plazo,
					descripcionesNacional,
					this.calcularTarifa(tarifaFijaNacional, plazo, precio, prima),
					"nacional",
					precio
				)
			);
			bancos.Add(
				new Models.Banco(
					"ScotiabankLogo.png", 
					tarifaFijaScotiabank,
					prima,
					plazos,
					plazo,
					descripcionesScotiabank,
					this.calcularTarifa(tarifaFijaScotiabank, plazo, precio, prima),
					"scotiabank",
					precio
				)
			);
			return View(bancos);
        }

		private double calcularTarifa(double tarijaFija, int plazo, double precio, double prima)
		{
			double tarifaFijaMensual = tarijaFija / 12;
			return Math.Round(
				(tarifaFijaMensual / 100 * (precio-prima)) / (1 - Math.Pow(1 + (tarifaFijaMensual / 100), -plazo))
			);
		}

		public ActionResult Recalcular(double prima, string bancoName)
		{
			foreach(var banco in bancos)
			{
				if (banco.name == bancoName)
				{
					banco.prima = prima;
					banco.cuotaMensual = this.calcularTarifa(
						banco.tasaFija, 
						banco.plazo, 
						banco.precio, 
						banco.prima
					);
				}
			}
			return View("Index", bancos);
		}

		public ActionResult RecalcularPlazo(
			int? bacPlazo, 
			int? nacionalPlazo, 
			int? scotiabankPlazo, 
			string bancoName
		)
		{
			int nuevoPlazo = bacPlazo != null ? Convert.ToInt32(bacPlazo) : 
				nacionalPlazo != null ? Convert.ToInt32(nacionalPlazo) : 
				scotiabankPlazo != null ? Convert.ToInt32(scotiabankPlazo) : 
				12;
			foreach (var banco in bancos)
			{
				if (banco.name == bancoName)
				{
					banco.plazo = nuevoPlazo;
					banco.cuotaMensual = this.calcularTarifa(
						banco.tasaFija,
						banco.plazo,
						banco.precio,
						banco.prima
					);
				}
			}
			return View("Index", bancos);
		}

		public ActionResult ExportToPdf(int? id)
		{
			if (bancos.Count > 0)
			{
				int pdfRowIndex = 1;
				string filename = "Calculo-De-Venta" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
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
				table.AddCell(new Phrase("Banco", font1));
				table.AddCell(new Phrase("Tasa Fija", font1));
				table.AddCell(new Phrase("Prima", font1));
				table.AddCell(new Phrase("Plazo", font1));
				table.AddCell(new Phrase("Cuota Mensual", font1));
				table.HeaderRows = 1;
				foreach (Banco banco in bancos)
				{
					table.AddCell(new Phrase(banco.name.ToString(), font2));
					table.AddCell(new Phrase(banco.tasaFija.ToString(), font2));
					table.AddCell(new Phrase(banco.prima.ToString(), font2));
					table.AddCell(new Phrase(banco.plazo.ToString(), font2));
					table.AddCell(new Phrase(banco.cuotaMensual.ToString(), font2));
					pdfRowIndex++;
				}
				document.Add(
					new Phrase(
						String.Concat("Total o Pecio: ", bancos[0].precio.ToString()),
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
	}
}