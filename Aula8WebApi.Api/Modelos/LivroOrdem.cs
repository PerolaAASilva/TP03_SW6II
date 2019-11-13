using System.Linq;
using System.Linq.Dynamic.Core;
using Aula8WebAPI.DAL.Model;

namespace Aula8WebAPI.Api.Modelos
{
	public class LivroOrdem
    {
        public string Ordenar { get; set; }
    }

    public static class LivroOrdemExtensions
    {
        public static IQueryable<Livro> AplicaOrdem(this IQueryable<Livro> query, LivroOrdem ordem)
        {
            if (!string.IsNullOrEmpty(ordem.Ordenar))
            {
                query = query.OrderBy(ordem.Ordenar);
            }
            return query;
        }
    }
}
