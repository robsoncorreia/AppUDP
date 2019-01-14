using AppUDP.Models;
using AppUDP.Pages.UDP;
using AppUDP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
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

        public ObservableCollection<Comando> Comandos { get; set; }

        Button BtnTestarBotao { get; set; }

        Button BtnEditarBotao { get; set; }

        public ICommand TestarCommand { get; set; }
        public ICommand ApagarBotaoCommand { get; set; }
        public ICommand EditarBotaoCommand { get; set; }
        private BotoesEditarPage botoesDetalhePage;
        public Comando Comando { get; set; }

        private readonly IUdpService _udpService;

        public BotoesEditarViewModel(BotoesEditarPage botoesDetalhePage, Comando comando)
        {
            Comandos = new ObservableCollection<Comando>();
            botoesDetalhePage.Title = comando.Id == 0 ? "CRIAR" : "EDITAR";
            BtnTestarBotao = botoesDetalhePage.FindByName<Button>("BtnTestarBotao");
            BtnEditarBotao = botoesDetalhePage.FindByName<Button>("BtnEditarBotao");
            BtnEditarBotao.Text = comando.Id == 0 ? "Criar" : "Editar";
            EtComando = botoesDetalhePage.FindByName<Entry>("EtComando");
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
            IPAddress address;

            DesabilitarBotao(BtnTestarBotao);

            if (!IPAddress.TryParse(EtIP.Text, out address))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "IP inválido.", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return;
            }
            if (string.IsNullOrEmpty(EtPort.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Porta", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return;
            }
            if (string.IsNullOrEmpty(EtComando.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Comando", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return;
            }
            await _udpService.Broadcast(comando: Comando.Send, port: Comando.Port, ip: Comando.IP);

            Comandos.Clear();

            foreach (var item in _udpService.Responses)
            {
                Comandos.Add(item);
            }
            HabilitarBotao(BtnTestarBotao);
        }

        private void HabilitarBotao(Button button)
        {
            button.IsEnabled = true;
        }

        private void DesabilitarBotao(Button button)
        {
            button.IsEnabled = false;
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
