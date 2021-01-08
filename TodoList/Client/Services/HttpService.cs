using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using TodoList.Shared.Auth;

namespace TodoList.Client.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;

        public HttpService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _http = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<HttpResponseMessage> Get(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendRequest(request);
        }

        public async Task<HttpResponseMessage> Post(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };

            return await SendRequest(request);
        }

        public async Task<HttpResponseMessage> Put(string uri, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
            };

            return await SendRequest(request);
        }

        public async Task<HttpResponseMessage> Patch<T>(string uri, JsonPatchDocument<T> value) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, uri)
            {
                Content = GeneratePatchContent(value)
            };

            return await SendRequest(request);
        }

        public async Task<HttpResponseMessage> Delete(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            return await SendRequest(request);
        }

        //helpers methods
        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            // add jwt auth header if user is logged in
            var user = await _localStorageService.GetItem<AuthenticateResponse>("user");
            if (user != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            return await _http.SendAsync(request);

        }

        private static StringContent GeneratePatchContent<T>(JsonPatchDocument<T> patchDocument) where T : class
        {
            var writer = new StringWriter();
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Serialize(writer, patchDocument);
            var json = writer.ToString();

            return new StringContent(json, Encoding.UTF8, "application/json-patch+json");
        }
    }
}
