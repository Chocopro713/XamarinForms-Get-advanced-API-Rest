using System;
namespace ObtenerAPIAvazando.Services.API_Rest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using Fusillade;
    using Plugin.Connectivity;
    using Plugin.Connectivity.Abstractions;
    using Polly;
    using Refit;
    using Xamarin.Forms;

    public class ApiManager : IApiManager
    {
        IUserDialogs _userDialogs = UserDialogs.Instance;
        IConnectivity _connectivity = CrossConnectivity.Current;
        IApiService<IObtenerAPI> obtenerApiAvanzadoAPI;
        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }
        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IObtenerAPI> obtenerAPI)
        {
            obtenerApiAvanzadoAPI = obtenerAPI;
            IsConnected = _connectivity.IsConnected;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.IsConnected;
            if (!e.IsConnected)
                ClearAllRunningTask();
        }

        // Cancel All Running Task
        private void ClearAllRunningTask()
        {
            var items = runningTasks.ToList();
            foreach (var item in items)
            {
                item.Value.Cancel();
                runningTasks.Remove(item.Key);
            }
        }

        /*
         * CONSUMO REST API
         */


        public async Task<HttpResponseMessage> NameOfEndPoint()
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(obtenerApiAvanzadoAPI.GetApi(Priority.UserInitiated).NameOfEndPoint());
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        /*
         * CONSUMO REST API
         */

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            if (!IsConnected)
            {
                var strngResponse = "No hay una conexión de red";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(3));
                return data;
            }

            IsReachable = await _connectivity.IsReachable(GlobalSetting.ApiHostName);

            if (!IsReachable)
            {
                var strngResponse = "No fue posible acceder al servidor";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                _userDialogs.Toast(strngResponse, TimeSpan.FromSeconds(3));
                return data;
            }

            data = await Policy
                .Handle<WebException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync
                (
                    retryCount: 1,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () =>
                {
                    var result = await task;

                    //Logout the user
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ClearAllRunningTask();
                        // await _userDialogs.AlertAsync("No se pudo validar su sesión. Por favor, vuelva a ingresar sus credenciales de acceso.");
                        MessagingCenter.Send<ApiManager>(this, MessagingKeys.ExpiredSession);
                    }
                    runningTasks.Remove(task.Id);

                    return result;
                });

            return data;
        }
    }
}
