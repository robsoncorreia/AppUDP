using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppUDP.Pages.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalPageMaster : ContentPage
    {
        public ListView ListView;

        public PrincipalPageMaster()
        {
            InitializeComponent();

            BindingContext = new PrincipalPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        private class PrincipalPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PrincipalPageMenuItem> MenuItems { get; set; }

            public PrincipalPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<PrincipalPageMenuItem>(new[]
                {
                    new PrincipalPageMenuItem { Id = 0, Title = "UDP BROADCAST", TargetType = typeof(BroadcastUDPPage)}
                });
            }

            #region INotifyPropertyChanged Implementation

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion INotifyPropertyChanged Implementation
        }
    }
}