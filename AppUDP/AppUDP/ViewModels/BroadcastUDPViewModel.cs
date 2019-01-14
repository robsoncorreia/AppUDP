using AppUDP.Models;
using AppUDP.Pages;
using AppUDP.Pages.UDP;
using AppUDP.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BroadcastUdpViewModel : BaseViewModel
    {
        private BroadcastUdpPage _broadcastUDPPage;

        private Comando _comandoSelected;

        private INavigation _navigation;

        private Stepper Tempo { get; set; }

        private Label TempoStepper { get; set; }

        private Button BtnBuscar { get; set; }

        public Comando ComandoSelected
        {
            get { return _comandoSelected; }
            set
            {
                _comandoSelected = value;
                if (value == null) return;
                AbrirPagina(value);
            }
        }

        private async void AbrirPagina(Comando value)
        {
            await _navigation.PushAsync(new BotoesEditarPage(value));
        }

        private readonly IUdpService udpService;

        public ICommand ActionBuscarCommand { get; set; }
        public ObservableCollection<Comando> Comandos { get; set; }

        public BroadcastUdpViewModel(BroadcastUdpPage broadcastUDPPage)
        {
            _broadcastUDPPage = broadcastUDPPage;

            Tempo = _broadcastUDPPage.FindByName<Stepper>("Tempo");

            Tempo.Value = 500;

            BtnBuscar = _broadcastUDPPage.FindByName<Button>("Buscar");

            TempoStepper = _broadcastUDPPage.FindByName<Label>("TempoStepper");

            TempoStepper.Text = Tempo.Value.ToString();

            Tempo.ValueChanged += Tempo_Changed;

            _navigation = _broadcastUDPPage.Navigation;

            Comandos = new ObservableCollection<Comando>();

            udpService = new UdpService();

            ActionBuscarCommand = new Command<string>(Buscar);
        }

        private void Tempo_Changed(object sender, ValueChangedEventArgs e)
        {
            TempoStepper.Text = e.NewValue.ToString();
        }

        private async void Buscar(string comando)
        {
            if (string.IsNullOrEmpty(comando))
            {
                await _broadcastUDPPage.DisplayAlert("Erro", "Preencha o campo Comando", "Fechar");
                return;
            }
            DesabilitarBotao(BtnBuscar);
            await udpService.Broadcast(comando: comando, timer: int.Parse(Tempo.Value.ToString()));
            HabilitarBotao(BtnBuscar);
            Comandos.Clear();

            foreach (var item in udpService.Responses)
            {
                Comandos.Add(item);
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
    }
}