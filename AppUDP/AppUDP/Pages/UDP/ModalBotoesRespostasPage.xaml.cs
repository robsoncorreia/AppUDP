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
    public partial class ModalBotoesRespostasPage : ContentPage
    {
        public ModalBotoesRespostasPage(System.Collections.ObjectModel.ObservableCollection<Models.Comando> respostasComandos)
        {
            InitializeComponent();

            BindingContext = new ModalBotoesRespostasViewModel(this, respostasComandos);
        }
    }
}