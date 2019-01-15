using AppUDP.Models;
using AppUDP.Pages.Master;
using AppUDP.Pages.UDP;
using AppUDP.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BotoesEditarViewModel : BaseViewModel
    {

        public ObservableCollection<Comando> Respostas { get; set; } = new ObservableCollection<Comando>();

        private Entry EtIP;

        private Entry EtPort;

        private Entry EtComando;

        public Label LbValorStepper { get; private set; }
        public ObservableCollection<Comando> Comandos { get; set; }
        public Stepper StTempoEspera { get; private set; }
        Button BtnTestarBotao { get; set; }

        Button BtnEditarBotao { get; set; }
        public Button BtnApagarBotao { get; private set; }
        public ICommand TestarCommand { get; set; }
        public ICommand ApagarBotaoCommand { get; set; }
        public ICommand EditarBotaoCommand { get; set; }

        public ICommand DuplicarBotaoCommand { get; set; }
        private BotoesEditarPage botoesDetalhePage;
        public Comando Comando { get; set; }

        // private readonly IUdpService _udpService;

        public BotoesEditarViewModel(BotoesEditarPage botoesDetalhePage, Comando comando)
        {
            Comando = comando;
            LbValorStepper = botoesDetalhePage.FindByName<Label>("LbValorStepper");
            Comandos = new ObservableCollection<Comando>();
            botoesDetalhePage.Title = comando.Id == 0 ? "CRIAR" : "EDITAR";
            StTempoEspera = botoesDetalhePage.FindByName<Stepper>("StTempoEspera");
            StTempoEspera.ValueChanged += StTempoEspera_ValueChanged;
            LbValorStepper.Text = StTempoEspera.Value.ToString();
            BtnTestarBotao = botoesDetalhePage.FindByName<Button>("BtnTestarBotao");
            BtnEditarBotao = botoesDetalhePage.FindByName<Button>("BtnEditarBotao");
            BtnApagarBotao = botoesDetalhePage.FindByName<Button>("BtnApagarBotao");
            BtnApagarBotao.IsVisible = Comando.Id != 0;
            BtnEditarBotao.Text = comando.Id == 0 ? "Criar" : "Editar";
            EtComando = botoesDetalhePage.FindByName<Entry>("EtComando");
            EtIP = botoesDetalhePage.FindByName<Entry>("EtIP");
            EtPort = botoesDetalhePage.FindByName<Entry>("EtPort");
            ApagarBotaoCommand = new Command(ApagarBotao);
            EditarBotaoCommand = new Command(EditarBotao);
            TestarCommand = new Command(Testar);
            DuplicarBotaoCommand = new Command(DuplicarBotao);
            this.botoesDetalhePage = botoesDetalhePage;
            UdpService.Invertido += UdpService_Invertido;
           /// _udpService = new UdpService();
        }

        private async void DuplicarBotao(object obj)
        {
            bool isValidos = await ValidarCampos();

            if (!isValidos) return;

            Comando.Id = 0;

            await App.Database.SaveItemAsync(Comando);

            await botoesDetalhePage.DisplayAlert("Dublicado", $"Botão dublicado com sucesso.", "Fechar");

            App.Current.MainPage = new PrincipalPage();
        }

        private void UdpService_Invertido()
        {
            Comandos.Clear();

            foreach (var item in UdpService.Responses)
            {
                Comandos.Add(item);
            }
        }

        private void StTempoEspera_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LbValorStepper.Text = e.NewValue.ToString();
        }

        private async void Testar(object obj)
        {
            DesabilitarBotao(BtnTestarBotao);

            bool isValidos = await ValidarCampos();

            if (!isValidos) return;
            
            await UdpService.Broadcast(comando: Comando.Send, port: Comando.Port, ip: Comando.IP, timer: int.Parse(StTempoEspera.Value.ToString()));

            Vibrar(30);

            HabilitarBotao(BtnTestarBotao);
        }

        private async Task<bool> ValidarCampos()
        {
            IPAddress address;
            if (!IPAddress.TryParse(EtIP.Text, out address))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "IP inválido.", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return false;
            }
            if (string.IsNullOrEmpty(EtPort.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Porta", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return false;
            }
            if (string.IsNullOrEmpty(EtComando.Text))
            {
                await botoesDetalhePage.DisplayAlert("Erro", "Preencha o campo Comando", "Fechar");
                HabilitarBotao(BtnTestarBotao);
                return false;
            }
            return true;
        }

        private void Vibrar(int duracao)
        {
            try
            {
                var duration = TimeSpan.FromMilliseconds(duracao);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
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
            
            await botoesDetalhePage.DisplayAlert("Edição", $"Botão {(Comando.Id == 0 ? "criado" : "editado")} com sucesso.", "Fechar");

            App.Current.MainPage = new PrincipalPage();
        }

        private async void ApagarBotao(object obj)
        {
            await App.Database.DeleteItemAsync(Comando);

            await botoesDetalhePage.DisplayAlert("Apagado", "Botão apagado com sucesso.", "Fechar");

            App.Current.MainPage = new PrincipalPage();
        }
    }
}
