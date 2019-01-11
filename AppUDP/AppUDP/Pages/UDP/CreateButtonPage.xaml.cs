using AppUDP.Models;
using AppUDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppUDP.Pages.UDP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateButtonPage : ContentPage
    {
        public CreateButtonPage(Receive receive)
        {
            InitializeComponent();

            BindingContext = new CreateButtonViewModel(receive, Navigation);
        }
    }
}