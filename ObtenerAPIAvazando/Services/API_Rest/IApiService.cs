using System;
namespace ObtenerAPIAvazando.Services.API_Rest
{
    using Fusillade;

    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
