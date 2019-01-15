using AppUDP.Models;
using AppUDP.Pages.UDP;
using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class ModalBotoesRespostasViewModel : BaseViewModel
    {

        public Command FecharModalCommand { get; set; }
        private ModalBotoesRespostasPage modalBotoesRespostasPage;
        public ObservableCollection<Comando> RespostasComandos { get; set; }

        private Comando _respostaComandoSelected;

        public Comando RespostaComandoSelected
        {
            get { return _respostaComandoSelected; }
            set
            {
                _respostaComandoSelected = value;
                if (value == null) return;
                CopiarComando(value);
            }
        }

        private async void CopiarComando(Comando value)
        {
            CrossClipboard.Current.SetText(value.ToString());
            await modalBotoesRespostasPage.DisplayAlert("Copiado", "Informações do comando copiadas.", "Fechar");
        }

        public ListView LwRespostasComandos { get; private set; }

        public ModalBotoesRespostasViewModel(ModalBotoesRespostasPage modalBotoesRespostasPage, ObservableCollection<Comando> respostasComandos)
        {
            this.modalBotoesRespostasPage = modalBotoesRespostasPage;
            RespostasComandos = respostasComandos;
            this.modalBotoesRespostasPage = modalBotoesRespostasPage;
            LwRespostasComandos = modalBotoesRespostasPage.FindByName<ListView>("LwRespostasComandos");
            FecharModalCommand = new Command(FecharModal);
        }

        private void FecharModal(object obj)
        {
            modalBotoesRespostasPage.Navigation.PopModalAsync();
        }
    }
}
