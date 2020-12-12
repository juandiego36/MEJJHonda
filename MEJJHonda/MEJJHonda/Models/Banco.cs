using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MejjHonda.Models
{
	public class Banco
	{
		public string logo;
		public double tasaFija;
		public double prima;
		public List<int> plazos;
		public int plazo;
		public List<string> descripciones;
		public double cuotaMensual;
		public string name;
		public double precio;

		public Banco(
			string _logo, 
			double _tasaFija, 
			double _prima, 
			List<int> _plazos, 
			int _plazo,
			List<string> _descripciones,
			double _cuotaMensual,
			string _name,
			double _precio
		)
		{
			this.logo = _logo;
			this.tasaFija = _tasaFija;
			this.prima = _prima;
			this.plazos = _plazos;
			this.plazo = _plazo;
			this.descripciones = _descripciones;
			this.cuotaMensual = _cuotaMensual;
			this.name = _name;
			this.precio = _precio;
		}
	}
}