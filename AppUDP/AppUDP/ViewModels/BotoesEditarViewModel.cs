using AppUDP.Models;
using AppUDP.Pages.UDP;
using AppUDP.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BotoesEditarViewModel : BaseViewModel
    {
        private Entry EtIP;

        private Entry EtPort;

        private Entry EtComando;

        public ICommand TestarCommand { get; set; }
        public ICommand ApagarBotaoCommand { get; set; }
        public ICommand EditarBotaoCommand { get; set; }
        private BotoesEditarPage botoesDetalhePage;
        public Comando Comando { get; set; }

        private readonly IUdpService _udpService;

        public BotoesEditarViewModel(BotoesEditarPage botoesDetalhePage, Comando comando)
        {
            EtComando  = botoesDetalhePage.FindByName<Entry>("EtComando");
            EtIP = botoesDetalhePage.FindByName<Entry>("EtIP");
            EtPort = botoesDetalhePage.FindByName<Entry>("EtPort");
            ApagarBotaoCommand = new Command(ApagarBotao);
            EditarBotaoCommand = new Command(EditarBotao);
            TestarCommand = new Command(Testar);
            this.botoesDetalhePage = botoesDetalhePage;
            this.Comando = comando;
            _udpService = new UdpService();
        }

        private async void Testar(object obj)
        {
            if (string.IsNullOrEmpty(EtIP.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo IP", "Fechar");
                return;
            }
            if (string.IsNullOrEmpty(EtPort.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Porta", "Fechar");
                return;
            }
            if (string.IsNullOrEmpty(EtComando.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Comando", "Fechar");
                return;
            }
            await _udpService.SendAsync(comando: Comando.Send, port: Comando.Port, ip: Comando.IP);
        }

        private async void EditarBotao(object obj)
        {
            await App.Database.SaveItemAsync(Comando);

            await botoesDetalhePage.DisplayAlert("Edição", "Botão editado com sucesso.", "Fechar");
        }

        private async void ApagarBotao(object obj)
        {
            await App.Database.DeleteItemAsync(Comando);
            await botoesDetalhePage.DisplayAlert("Apagado", "Botão apagado com sucesso.", "Fechar");
            await botoesDetalhePage.Navigation.PushAsync(new BotoesPage());
        }
    }
}
