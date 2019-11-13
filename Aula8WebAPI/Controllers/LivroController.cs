using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aula8WebAPI.DAL.Livros;
using Aula8WebAPI.DAL.Model;


namespace Aula8WebAPI.WebApp.Controllers
{
	[Authorize]
	public class LivrosController : Controller
	{
		private readonly IRepository<Livro> _repo;

		public LivrosController(IRepository<Livro> repository)
		{
			_repo = repository;
		}

		[HttpGet]
		public IActionResult Novo()
		{
			return View(new LivroUpload());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Novo(LivroUpload model)
		{
			if (ModelState.IsValid)
			{
				_repo.Incluir(model.ToLivro());
				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ImagemCapa(int id)
		{
			byte[] img = _repo.All
			.Where(l => l.Id == id)
			.Select(l => l.ImagemCapa)
			.FirstOrDefault();
			if (img != null)
			{
				return File(img, "image/png");
			}
			return File("~/images/capas/capa-vazia.png", "image/png");
		}

		[HttpGet]
		public IActionResult Detalhes(int id)
		{
			var model = _repo.Find(id);
			if (model == null)
			{
				return NotFound();
			}
			return View(model.ToModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Detalhes(LivroUpload model)
		{
			if (ModelState.IsValid)
			{
				var livro = model.ToLivro();
				if (model.Capa == null)
				{
					livro.ImagemCapa = _repo.All
					.Where(l => l.Id == livro.id)
					.Select(l => l.ImagemCapa)
					.FirstOrDefault();
				}
				_repo.Alterar(livro);
				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Excluir(int id)
		{
			var model = _repo.Find(id);
			if (model == null)
			{
				return NotFound();
			}
			_repo.Excluir(model);
			return RedirectToAction("Index", "Home");
		}
	}
}