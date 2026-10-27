using _6_webApi.Models;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update.Internal;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        public enum Mode {Create, Put, Patch};
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<Hotel> hotels { get; set; } = new();
        private Hotel? _selectedHotel;
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value; OnPropertyChanged();
            }
        }

        private string _cityFilter = "";
        public string CityFilter
        {
            get => _cityFilter;
            set
            {
                _cityFilter = value; OnPropertyChanged();
            }
        }
        private string _minRatingFilter = "";
        public string MinRatingFilter
        {
            get => _minRatingFilter;
            set
            {
                _minRatingFilter = value; OnPropertyChanged();
            }
        }
        public HttpClient _client = new HttpClient();
        public HotelApi _api;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _client.BaseAddress = new Uri("https://localhost:7126/");
            _api = new HotelApi(_client);
        }

        private async System.Threading.Tasks.Task LoadHotels()
        {
            try
            {
                decimal? minRating = null;
                if (!string.IsNullOrWhiteSpace(MinRatingFilter))
                {
                    if (!decimal.TryParse(MinRatingFilter, out var r) || r < 0 || r > 5)
                    {
                        MessageBox.Show("Минимальный рейтинг должен быть числом от 0 до 5");
                        return;
                    }
                    minRating = r;
                }

                var list = await _api.GetHotels(string.IsNullOrWhiteSpace(CityFilter) ? null : CityFilter, minRating);
                hotels.Clear();
                foreach (var h in list)
                {
                    hotels.Add(h);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка: {exception}");
            }
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedHotel !=null)
            {
                if (SelectedHotel != null) GoToEdit(SelectedHotel, Mode.Put);
            }
        }

        private async void Getall_Click(object sender, RoutedEventArgs e)
        {
            await LoadHotels();
        }

        private async void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadHotels();

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            GoToEdit(null, Mode.Create);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHotel == null)
            {
                MessageBox.Show("Выберите отель.");
                return;
            }

            GoToEdit(SelectedHotel, Mode.Patch);
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedHotel == null)
            {
                MessageBox.Show("Выберите отель.");
                return;
            }

            GoToEdit(SelectedHotel, Mode.Put);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedHotel == null)
                {
                    MessageBox.Show("Выберите отель.");
                    return;
                }

                if (MessageBox.Show($"Удалить {SelectedHotel.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    return;

                await _api.Delete(SelectedHotel.Id, SelectedHotel);
                await LoadHotels();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex}");
            }
        }

        private void GoToEdit(Hotel? hotel, Mode mode)
        {
            var newPage = new Pages.EditPage(hotel, _api, mode);
            MainFrame.Navigate(newPage);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}