using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ObtenerAPIAvazando.ViewModels
{
    public class GetApiViewModel : BaseViewModel
    {
        #region Attributes
        #endregion Attributes

        #region Properties
        public ICommand GetAPICommand { get; set; }
        #endregion Properties

        #region Constructor
        public GetApiViewModel()
        {
            this.GetAPICommand = new Command(OnGetAPICommand);
        }
        #endregion Constructor

        #region Methods
        #endregion Methods

        #region Commands
        /// <summary>
        /// Get the information when the user presses the button 
        /// </summary>
        private async void OnGetAPICommand()
        {
            if (this.IsBusy)
                return;
            this.IsBusy = true;

            PageDialog.ShowLoading("Comprobando Datos");

            var NameApi = await ApiManager.NameOfEndPoint();
            if (!NameApi.IsSuccessStatusCode)
            {
                await PageDialog.AlertAsync("No se pudo obtener los datos", "Error", "Aceptar");
                this.IsBusy = false;
                this.PageDialog.HideLoading();
                return;
            }
            var jsonNameResponse = await NameApi.Content.ReadAsStringAsync();
            var NameResponse = await Task.Run(() => JsonConvert.DeserializeObject<GetApiViewModel>(jsonNameResponse));

            this.PageDialog.HideLoading();
            this.IsBusy = false;
        }
        #endregion Commands
    }
}
