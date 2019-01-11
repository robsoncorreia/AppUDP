using AppUDP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppUDP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BroadcastUDPPage : ContentPage
    {
        public BroadcastUDPPage()
        {
            InitializeComponent();

            BindingContext = new BroadcastUDPViewModel(this.Navigation);
        }

        private void Tempo_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            TempoStepper.Text = e.NewValue.ToString();
        }
    }
}