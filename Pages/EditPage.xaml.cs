using _6_webApi.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _7_wpf.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page, INotifyPropertyChanged
    {
        readonly HotelApi _api;
        readonly Mode _mode;
        readonly int _hotelID;

        string _name = "";
        string _city = "";
        decimal _rating;
        decimal _pricePerNight;
        bool _isAvailable;

        public EditPage(Hotel? hotel, HotelApi api, Mode mode)
        {
            InitializeComponent();
            _api = api;
            _mode = mode;
            _hotelID = hotel?.Id ?? 0;

            if (hotel != null)
            {
                _name = hotel.Name;
                _city = hotel.City;
                _rating = hotel.Rating;
                _pricePerNight = hotel.PricePerNight;
                _isAvailable = hotel.IsAvailable;
            }

            DataContext = this;

        }

        public string Header => _mode switch
        {
            Mode.Create => "Создание",
            Mode.Put => "Изменение",
            Mode.Patch => "Патч",
            _ => ""
        };

        public bool IsReadOnly => _mode == Mode.Patch;
        public string HotelName
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(); }
        }

        public decimal Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(); }
        }

        public decimal PricePerNight
        {
            get => _pricePerNight;
            set { _pricePerNight = value; OnPropertyChanged(); }
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; OnPropertyChanged(); }
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hotel = new Hotel
                {
                    Id = _hotelID,
                    Name = HotelName,
                    City = City,
                    Rating = Rating,
                    PricePerNight = PricePerNight,
                    IsAvailable = IsAvailable
                };


                 switch (_mode)
                {
                    case Mode.Create: { await _api.CreateHotel(hotel); break; }
                    case Mode.Put: { await _api.Update(_hotelID, hotel); break; }
                    case Mode.Patch: {
                            await _api.Update(_hotelID, new HotelPatch
                            {
                                Rating = Rating,
                                PricePerNight = PricePerNight,
                                IsAvailable = IsAvailable
                            });
                            break;
                        }
                };

                NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка:{ex}");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        
    }
}
