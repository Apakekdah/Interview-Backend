using Microsoft.Extensions.Configuration;
using Moduit.Interview.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Moduit.Interview.Libs
{
    public class Requester : IRequester
    {
        private readonly IConfiguration configuration;

        public Requester(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<T> GetRequest<T>(string path) where T : class
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var host = configuration["TheHost"];
            if (string.IsNullOrEmpty(host))
                throw new NullReferenceException("TheHost has missing value");

            UriBuilder uriB = new UriBuilder(host);
            uriB.Path = path;

            using (var http = new HttpClient())
            {
                using (var respon = await http.GetAsync(uriB.Uri))
                {
                    using (var ensure = respon.EnsureSuccessStatusCode())
                    {
                        using (var content = ensure.Content)
                        {
                            var tmp = await content.ReadAsStringAsync();
                            return (T)JsonSerializer.Deserialize(tmp, typeof(T), new System.Text.Json.JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = false,
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            });
                        }
                    }
                }
            }
        }
    }
}
