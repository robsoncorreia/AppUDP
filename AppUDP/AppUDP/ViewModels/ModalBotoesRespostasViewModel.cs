using AppUDP.Models;
using AppUDP.Pages.UDP;
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

        private Comando _respostaComandoSelecte;

        public Comando RespostaComandoSelecte
        {
            get { return _respostaComandoSelecte; }
            set { _respostaComandoSelecte = value; }
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
