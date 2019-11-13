using System.Collections.Generic;
using System.Linq;
using Aula8WebAPI.DAL.Model;

namespace Aula8WebAPI.Modelos
{
	public class ListaLeitura
	{
		public string Tipo { get; set; }
		public IEnumerable<LivroApi> Livros { get; set; }
	}

	public static class TipoListaLeituraExtensions
	{
		private static Dictionary<string, TipoListaLeitura> mapa =
			 new Dictionary<string, TipoListaLeitura>
			 {
					 { "ParaLer", TipoListaLeitura.ParaLer },
					 { "Lendo", TipoListaLeitura.Lendo },
					 { "Lidos", TipoListaLeitura.Lidos }
			 };

		public static string ParaString(this TipoListaLeitura tipo)
		{
			return mapa.First(s => s.Value == tipo).Key;
		}

		public static TipoListaLeitura ParaTipo(this string texto)
		{
			return mapa.First(t => t.Key == texto).Value;
		}
	}
	public enum TipoListaLeitura
	{
		ParaLer,
		Lendo,
		Lidos
	}
}
