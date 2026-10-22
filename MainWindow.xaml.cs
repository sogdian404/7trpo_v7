using _6_webApi.Models;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update.Internal;

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Hotel> hotels { get; set; } = new();
        public Hotel? hotel { get; set; } = null;
        public HttpClient client = new HttpClient();
        public HotelApi api;
        
        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:7126/");
            api = new HotelApi(client);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private async void Getall_Click(object sender, RoutedEventArgs e)
        {
            hotels = await api.GetHotels();
        }

        private async void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            hotels = await api.GetHotels();

        }


    }
}