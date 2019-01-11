using AppUDP.Models;
using AppUDP.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppUDP.ViewModels
{
    public class CreateButtonViewModel : BaseViewModel
    {
        private readonly IUdpService _udpService;
        public ICommand CloseModalCommand { get; set; }
        public ICommand CreateButtonCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public Receive Receive { get; set; }
        public Comando Comando { get; set; }
        private INavigation _navigation;

        public CreateButtonViewModel(Receive receive, INavigation navigation)
        {
            _udpService = new UdpService();
            Comando = new Comando();
            CloseModalCommand = new Command(CloseModal);
            CreateButtonCommand = new Command(CreateButton);
            TestCommand = new Command(Test);
            Receive = receive;
            _navigation = navigation;
        }

        private async void Test(object obj)
        {
            await _udpService.SendAsync(comando: Comando?.Send, port: int.Parse(Receive?.Port), ip: Receive?.IP);
        }

        private void CreateButton(object obj)
        {
        }

        private async void CloseModal(object obj)
        {
            await _navigation.PopModalAsync();
        }
    }
}