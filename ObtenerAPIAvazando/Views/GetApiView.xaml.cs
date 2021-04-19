using System;
using System.Collections.Generic;
using ObtenerAPIAvazando.ViewModels;
using Xamarin.Forms;

namespace ObtenerAPIAvazando.Views
{
    public partial class GetApiView : ContentPage
    {
        public GetApiView()
        {
            InitializeComponent();

            BindingContext = new GetApiViewModel();
        }
    }
}
