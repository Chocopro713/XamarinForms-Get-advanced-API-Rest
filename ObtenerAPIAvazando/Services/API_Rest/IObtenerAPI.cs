using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace ObtenerAPIAvazando.Services.API_Rest
{
    public interface IObtenerAPI
    {
        [Get("/api/Put_your_endPoint")]
        Task<HttpResponseMessage> NameOfEndPoint();
    }
}
