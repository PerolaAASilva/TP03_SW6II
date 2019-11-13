using Aula8WebAPI.DAL.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Aula8WebAPI.Formatters
{
    public class LivroCsvFormatter : TextOutputFormatter
    {
        private const string TEXT_CSV = "text/csv";
        private const string APP_CSV = "application/csv";

        public LivroCsvFormatter()
        {
            var textCsvMideaType = MediaTypeHeaderValue.Parse(TEXT_CSV);
            var appCsvMideaType = MediaTypeHeaderValue.Parse(APP_CSV);
            SupportedMediaTypes.Add(textCsvMideaType);
            SupportedMediaTypes.Add(appCsvMideaType);
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(LivroApi);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var livroEmCsv = "";

            if (context.Object is LivroApi)
            {
                var livro = context.Object as LivroApi;

                livroEmCsv =
                      $"{livro.Titulo};" +
                      $"{livro.Subtitulo};" +
                      $"{livro.Autor};" +
                      $"{livro.Lista}";
            }

            using (var escritor = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return escritor.WriteAsync(livroEmCsv);
            }
        }
    }
}
