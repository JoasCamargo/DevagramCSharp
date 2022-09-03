using System.Net.Http.Headers;
using DevagramCSharp.Dtos;

namespace DevagramCSharp.Services
{
    public class CosmicService
    {
        public string EnviarImagem(ImagemDto imagemdto)
        {
            Stream imagem = imagemdto.Imagem.OpenReadStream();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ZFK36Mxb8EfJkjb4KvpyuzoUe9I6YhOhFirZtBKM24vQ4FEh26");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            var conteudo = new MultipartFormDataContent
            {
                { new StreamContent(imagem), "media", imagemdto.Nome }
            };

            request.Content = conteudo;
            var retornoreq = client.PostAsync("https://upload.cosmicjs.com/v2/buckets/devagrambucket-devagram/media", request.Content).Result;

            var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRepostaDto>();

            return urlretorno.Result.media.url;
        }
    }
}
