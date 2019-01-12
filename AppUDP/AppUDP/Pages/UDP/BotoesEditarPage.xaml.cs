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
    public partial class BotoesEditarPage : ContentPage
    {
        public BotoesEditarPage(Comando comando)
        {
            InitializeComponent();

            BindingContext = new BotoesEditarViewModel(this, comando);
        }
        public BotoesEditarPage()
        {
            InitializeComponent();

            BindingContext = new BotoesEditarViewModel(this, new Comando());
        }
    }
}