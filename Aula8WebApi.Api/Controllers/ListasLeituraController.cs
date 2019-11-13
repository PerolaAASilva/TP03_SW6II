using Aula8WebAPI.DAL.Livros;
using Aula8WebAPI.DAL.Model;
using Aula8WebAPI.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Lista = Aula8WebAPI.Modelos.ListaLeitura;

namespace Aula8WebAPI.Api.Controllers
{
	/// <summary>
	/// Para utilização de acesso pela query string, deve-se remover da Route a propriedade de versão, com isso é possível criar
	/// que a rota é acessada pelo parametro ?api-version=1.0 [Por examplo]
	/// </summary>
	[Authorize]
	[ApiVersion("1.0")]
	[ApiVersion("2.0")]
	[Route("api/v{version:apiversion}/[controller]")]
	[ApiController]
	public class ListasLeituraController : ControllerBase
	{

		private readonly IRepository<Livro> _repo;

		public ListasLeituraController(IRepository<Livro> repository) => _repo = repository;

		private Lista CriarLista(TipoListaLeitura tipo)
		{
			return new Lista
			{
				Tipo = tipo.ParaString(),
				Livros = _repo.All
				 .Where(l => l.Lista == tipo)
				 .Select(l => l.ToApi())
				 .ToList()
			};
		}

		[HttpGet]
		public IActionResult TodasListas()
		{
			Lista paraLer = CriarLista(TipoListaLeitura.ParaLer);
			Lista lendo = CriarLista(TipoListaLeitura.Lendo);
			Lista lidos = CriarLista(TipoListaLeitura.Lidos);
			var colecao = new List<Lista> { paraLer, lendo, lidos };
			return Ok(colecao);
		}

		[HttpGet("{tipo}")]
		public IActionResult Recuperar(TipoListaLeitura tipo)
		{
			var lista = CriarLista(tipo);
			return Ok(lista);
		}
	}
}