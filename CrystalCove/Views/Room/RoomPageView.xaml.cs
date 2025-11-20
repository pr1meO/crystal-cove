using CrystalCove.Models;
using CrystalCove.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrystalCove.Views;

public partial class RoomPageView : Page
{
    public RoomPageView()
    {
        InitializeComponent();
        RefreshRoomData();
    }

    // Метод для обновления данных комнаты
    public void RefreshRoomData()
    {
        RoomDataGrid.ItemsSource = RoomService.GetAllRoom();
        UpdateRoomCount();
    }

    // Метод для поиска родительского элемента определенного типа
    private static T FindPerent<T>(DependencyObject child) where T : DependencyObject
    {
        DependencyObject perentObject = VisualTreeHelper.GetParent(child);
        if (perentObject == null)
        {
            return null;
        }
        T perent = perentObject as T;
        return perent ?? FindPerent<T>(perentObject);
    }

    // Обработчик события нажатия на кнопку для добавления новой комнаты
    private void AddRoom_Click(object sender, RoutedEventArgs e)
    {
        var hotelHomeView = FindPerent<HotelHomeView>(this);
        hotelHomeView?.ShowOverlay();

        AddRoomView addRoomView = new(RoomDataGrid);

        addRoomView.ShowDialog();

        hotelHomeView?.HideOverlay();

        RefreshRoomData();
    }

    // Обработчик события нажатия на кнопку для обновления комнаты
    private void UpdateRoom_Click(object sender, RoutedEventArgs e)
    {
        var hotelHomeView = FindPerent<HotelHomeView>(this);
        hotelHomeView?.ShowOverlay();

        var selectedRoom = RoomDataGrid.SelectedItem as Room;

        // Проверяем, выбрана ли комната
        if (selectedRoom != null)
        {
            // Проверяем, забронирована ли комната
            if (selectedRoom.StatusBooking)
            {
                MessageBox.Show("Нельзя обновить забронированную комнату!", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
                hotelHomeView?.HideOverlay();
                return;
            }
            UpdateRoomView updateRoomView = new(RoomDataGrid);
            updateRoomView.ShowDialog();
            hotelHomeView?.HideOverlay();
        }
    }

    // Обработчик события нажатия на кнопку для удаления комнаты
    private void DeleteRoom_Click(object sender, RoutedEventArgs e)
    {
        var selectedRoom = RoomDataGrid.SelectedItem as Room;
        if (selectedRoom != null)
        {
            // Проверяем, забронирована ли комната
            if (!selectedRoom.StatusBooking)
            {
                // Если комната не забронирована, удаляем её
                RoomService.DeleteRoom(selectedRoom);
                RefreshRoomData();
            }
            else
            {
                MessageBox.Show("Нельзя удалить забронированную комнату!", "Ошибка", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Метод для обновления значения с количеством свободных комнат
    private void UpdateRoomCount()
    {
        using AppDbContext hotelDbContext = new();

        // Получите количество свободных комнат из БД и обновите значение
        RoomCount.Text = hotelDbContext.Rooms.Count(room => room.StatusBooking == false).ToString();
    }
}
