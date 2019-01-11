using AppUDP.Models;
using AppUDP.Pages.UDP;
using AppUDP.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BroadcastUDPViewModel : BaseViewModel
    {
        private Receive _receiveSelected;

        private INavigation _navigation;

        public Receive ReceiveSelected
        {
            get { return _receiveSelected; }
            set
            {
                _receiveSelected = value;
                if (value == null) return;
                OpenModal(value);
            }
        }

        private async void OpenModal(Receive value)
        {
            await _navigation.PushModalAsync(new NavigationPage(new BroadcastUdpDetailPage(value)), true);
        }

        private readonly IUdpService udpService;

        public ICommand ActionBuscarCommand { get; set; }
        public ObservableCollection<Receive> Receives { get; set; }

        public BroadcastUDPViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Receives = new ObservableCollection<Receive>();
            udpService = new UdpService();

            ActionBuscarCommand = new Command<string>(Buscar);
        }

        private async void Buscar(string comando)
        {
            if (string.IsNullOrEmpty(comando)) return;
            await udpService.Broadcast(comando: comando);

            Receives.Clear();

            foreach (var item in udpService.Responses)
            {
                Receives.Add(item);
            }
        }
    }
}