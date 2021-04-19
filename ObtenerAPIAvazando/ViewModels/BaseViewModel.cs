using System;
using Acr.UserDialogs;
using ObtenerAPIAvazando.Services.API_Rest;

namespace ObtenerAPIAvazando.ViewModels
{
    public class BaseViewModel
    {
        #region Attributes API
        IApiService<IObtenerAPI> obtenerAPI = new ApiService<IObtenerAPI>(GlobalSetting.ApiUrl);
        public IApiManager ApiManager;
        public bool IsBusy { get; set; }
        #endregion Attributes API

        #region Attributes
        public IUserDialogs PageDialog = UserDialogs.Instance;
        #endregion Attributes

        #region Constructor
        public BaseViewModel()
        {
            this.IsBusy = false;
            this.ApiManager = new ApiManager(obtenerAPI);
        }
        #endregion Constructor
    }
}
