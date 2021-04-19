using System;
using ObtenerAPIAvazando.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObtenerAPIAvazando
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new GetApiView();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
