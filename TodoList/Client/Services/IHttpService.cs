using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoList.Client.Services
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> Get(string uri);
        Task<HttpResponseMessage> Post(string uri, object value);
        Task<HttpResponseMessage> Put(string uri, object value);
        Task<HttpResponseMessage> Patch<T>(string uri, JsonPatchDocument<T> value) where T : class;
        Task<HttpResponseMessage> Delete(string uri);
    }
}
