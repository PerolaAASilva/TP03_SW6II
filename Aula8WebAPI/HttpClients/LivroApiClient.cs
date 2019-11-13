using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Aula8WebAPI.DAL.Model;
using Aula8WebAPI.Modelos;



namespace Aula8WebAPI.WebApp.HttpClients
{
	public class LivroApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _accessor;

		public LivroApiClient(HttpClient httpClient, IHttpContextAccessor accessor)
		{
			_httpClient = httpClient;
			_accessor = accessor;
		}

		private void AddBearerToken()
		{
			var token = _accessor.HttpContext.User.Claims.First(c => c.Type == "Token").Value;
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		public async Task<LivroApi> GetLivroAsync(int id)
		{
			AddBearerToken();
			var resposta = await _httpClient.GetAsync($"Livros/{id}");
			resposta.EnsureSuccessStatusCode();
			return await resposta.Content.ReadAsAsync<LivroApi>();
		}

		public async Task<byte[]> GetCapaLivroAsync(int id)
		{
			AddBearerToken();
			HttpResponseMessage resposta = await _httpClient.GetAsync($"Livros/{id}/capa");
			resposta.EnsureSuccessStatusCode();
			return await resposta.Content.ReadAsByteArrayAsync();
		}

		public async Task<List<ListaLeitura>> GetListaLeituraAsync(TipoListaLeitura tipo)
		{
			AddBearerToken();
			var resposta = await _httpClient.GetAsync($"listasleitura/{tipo}");
			resposta.EnsureSuccessStatusCode();
			return await resposta.Content.ReadAsAsync<List<ListaLeitura>>();
		}

		public async Task DeleteLivroAsync(int id)
		{
			AddBearerToken();
			HttpResponseMessage resposta = await _httpClient.DeleteAsync($"Livros/{id}");
			resposta.EnsureSuccessStatusCode();
		}

		public async Task PostLivroAsync(LivroUpload livro)
		{
			AddBearerToken();
			HttpContent content = CreateMultipartContent(livro.ToLivro());
			var resposta = await _httpClient.PostAsync("livros", content);
			resposta.EnsureSuccessStatusCode();
			if (resposta.StatusCode != System.Net.HttpStatusCode.Created)
			{
				throw new InvalidOperationException("Código de Status Http 201 esperado!");
			}
		}

		public async Task PutLivroAsync(LivroUpload livro)
		{
			AddBearerToken();
			HttpContent content = CreateMultipartContent(livro.ToLivro());
			var resposta = await _httpClient.PutAsync("livros", content);
			resposta.EnsureSuccessStatusCode();
			if (resposta.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new InvalidOperationException("Código de Status Http 200 esperado!");
			}
		}

		private HttpContent CreateMultipartContent(Livro livro)
		{
			MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
			var content = multipartFormDataContent;

			content.Add(new StringContent(livro.Titulo), "titulo");
			content.Add(new StringContent(livro.Lista.ParaString()), ("lista"));

			if (livro.Id > 0)
				content.Add(new StringContent(Convert.ToString(livro.Id)), ("id"));

			if (!string.IsNullOrEmpty(livro.Subtitulo))
				content.Add(new StringContent(livro.Subtitulo), ("subtitulo"));

			if (!string.IsNullOrEmpty(livro.Resumo))
				content.Add(new StringContent(livro.Resumo), ("resumo"));

			if (!string.IsNullOrEmpty(livro.Autor))
				content.Add(new StringContent(livro.Autor), ("autor"));

			if (livro.ImagemCapa != null)
			{
				var imageContent = new ByteArrayContent(livro.ImagemCapa);
				imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
				content.Add(imageContent, ("capa"), ("capa.png"));
			}

			return content;
		}
	}
}

