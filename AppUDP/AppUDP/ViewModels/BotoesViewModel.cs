using AppUDP.Models;
using AppUDP.Pages.UDP;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BotoesViewModel : BaseViewModel
    {
        private ListView _lwComandos;

        private BotoesPage _botoesPage;

        private Comando _selectedComando;

        public Comando SelectedComando
        {   
            get { return _selectedComando; }
            set {
                _selectedComando = value;
                if (value == null) return;
                AbrirBotaoDetalhePage(value);
                _lwComandos.SelectedItem = null;
            }
        }

        private async void AbrirBotaoDetalhePage(Comando value)
        {
            await _botoesPage.Navigation.PushAsync(new BotoesEditarPage(value));
        }

        public List<Comando> Comandos { get; set; } = new List<Comando>();

        public BotoesViewModel(BotoesPage botoesPage)
        {
            _botoesPage = botoesPage;
            _lwComandos = _botoesPage.FindByName<ListView>("LwComandos");
            Comandos = App.Database.GetItemsAsync().Result;
        }
    }
}