using System;
using System.Collections.Generic;
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
			double prima = 10 * precio / 100;
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
	}
}