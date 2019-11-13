using Aula8WebAPI.Modelos;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace Aula8WebAPI.DAL.Model
{
    public static class LivrosExtensions
    {
        public static byte[] ConvertToBytes(this IFormFile image)
        {
            if (image == null)
            {
                return null;
            }
            using (var inputStream = image.OpenReadStream())
            using (var stream = new MemoryStream())
            {
                inputStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static byte[] ConvertToBytes(this Livro livro)
        {
            //if (image == null)
            //{
            //    Image img = Image.FromFile(@"C:\Users\lg.LUIS\source\repos\Aula8WebAPI\Aula8WebAPI\wwwroot\images\capas\capa-vazia.png");
            //    byte[] arr;
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        img.Save(ms, ImageFormat.Jpeg);
            //        arr = ms.ToArray();
            //    }
            //}
            //if (livro.ImagemCapa == null)
            //{
            //    return null;
            //}
            //using (var inputStream = livro.ImagemCapa)
            //using (var stream = new MemoryStream())
            //{
            //    inputStream.CopyTo(stream);
            //    return stream.ToArray();
            //}
            return null;
        }

        public static Livro ToLivro(this LivroUpload model)
        {
            return new Livro
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Subtitulo = model.Subtitulo,
                Resumo = model.Resumo,
                Autor = model.Autor,
                ImagemCapa = model.Capa.ConvertToBytes(),
                Lista = model.Lista
            };
        }

        public static LivroApi ToApi(this Livro livro)
        {
            return new LivroApi
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                ImagemCapa = $"/Api/Livros/{livro.Id}/Capa",
                Lista = livro.Lista.ParaString()
            };
        }

        public static LivroUpload ToModel(this Livro livro)
        {
            return new LivroUpload
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                Lista = livro.Lista
            };
        }

        public static LivroUpload ToUpload(this LivroApi livro)
        {
            return new LivroUpload
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                Lista = livro.Lista.ParaTipo()
            };
        }



    }
}

