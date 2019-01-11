using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AppUDP.Models;
using AppUDP.Pages.UDP;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class BroadcastUdpDetailViewModel
    {
        private INavigation _navigation;
        public ICommand CloseModalCommand { get; set; }

        public ICommand CriarBotaoCommand { get; set; }

        public Receive Receive { get; set; }

        public BroadcastUdpDetailViewModel(Receive value, INavigation navigation)
        {
            CloseModalCommand = new Command(CloseModal);

            CriarBotaoCommand = new Command(CriarBotao);
            Receive = value;
            _navigation = navigation;
        }

        private async void CriarBotao(object obj)
        {
            await _navigation.PushModalAsync(new CreateButtonPage(Receive));
        }

        private void CloseModal(object obj)
        {
            _navigation.PopModalAsync();
        }
    }
}