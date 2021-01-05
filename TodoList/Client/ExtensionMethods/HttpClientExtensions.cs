using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace TodoList.Client.ExtensionMethods
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client,
            string requestUri,
            JsonPatchDocument<T> patchDocument)
            where T : class
        {
            var writer = new StringWriter();
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, patchDocument);
            var json = writer.ToString();

            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            return await client.PatchAsync(requestUri, content);
        }
    }
}
