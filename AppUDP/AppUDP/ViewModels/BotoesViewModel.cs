using AppUDP.Models;
using AppUDP.Pages.UDP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BotoesViewModel : BaseViewModel
    {
  
        public ObservableCollection<Comando> RespostasComandos { get; set; }

        private Comando _respostaComandoSelected;

        public Comando RespostaComandoSelected
        {
            get { return _respostaComandoSelected; }
            set { _respostaComandoSelected = value; }
        }


        private ListView _lwComandos;

        private BotoesPage _botoesPage;

        private Comando _selectedComando;

        public Comando SelectedComando
        {
            get { return _selectedComando; }
            set
            {
                _selectedComando = value;
                if (value == null) return;
                AbrirBotaoDetalhePage(value);
                _lwComandos.SelectedItem = null;
            }
        }

        private async void AbrirBotaoDetalhePage(Comando value)
        {
            await _botoesPage.Navigation.PushAsync(new BotoesEditarPage(value) { Title = "Editar" });
        }

        public List<Comando> Comandos { get; set; } = new List<Comando>();

        public BotoesViewModel(BotoesPage botoesPage)
        {
            RespostasComandos = new ObservableCollection<Comando>();
            _botoesPage = botoesPage;
            _lwComandos = _botoesPage.FindByName<ListView>("LwComandos");

            PegarComandos();
            
        }

        private  void PegarComandos()
        {
            Comandos =  App.Database.GetItemsAsync().Result;
        }
    }
}