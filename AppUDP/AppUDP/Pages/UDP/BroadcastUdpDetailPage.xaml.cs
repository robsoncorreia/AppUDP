using AppUDP.Models;
using AppUDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppUDP.Pages.UDP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BroadcastUdpDetailPage : ContentPage
    {
        public BroadcastUdpDetailPage(Receive value)
        {
            InitializeComponent();
            BindingContext = new BroadcastUdpDetailViewModel(value, Navigation);
        }
    }
}