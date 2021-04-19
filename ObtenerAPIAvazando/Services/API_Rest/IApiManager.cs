using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ObtenerAPIAvazando.Services.API_Rest
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> NameOfEndPoint();
    }
}
