using AppUDP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppUDP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BroadcastUdpPage : ContentPage
    {
        public BroadcastUdpPage()
        {
            InitializeComponent();

            BindingContext = new BroadcastUdpViewModel(this);
        }
    }
}