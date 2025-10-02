using ITServiceApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ITServiceApp.UI
{
    public partial class MainWindow : Window
    {
        private List<ServiceRequest> _requests;
        private List<ServiceRequest> _filteredRequests;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
            InitializeFilters();
        }

        private void InitializeData()
        {
            // Тестовые данные
            _requests = new List<ServiceRequest>
            {
                new ServiceRequest("REQ001", "Компьютер", "Dell Optiplex 7070",
                    "Не включается", "Иванов Иван", "+7(999)123-45-67"),
                new ServiceRequest("REQ002", "Принтер", "HP LaserJet Pro",
                    "Зажевывает бумагу", "Петров Петр", "+7(999)765-43-21")
            };

            _requests[0].Status = RequestStatus.InProgress;
            _requests[0].AssignedEngineer = "Сергеев А.В.";

            _filteredRequests = _requests;
            RequestsGrid.ItemsSource = _filteredRequests;
        }

        private void InitializeFilters()
        {
            StatusFilterComboBox.Items.Add("Все статусы");
            StatusFilterComboBox.Items.Add("Новая");
            StatusFilterComboBox.Items.Add("В процессе");
            StatusFilterComboBox.Items.Add("Ожидание запчастей");
            StatusFilterComboBox.Items.Add("Завершена");
            StatusFilterComboBox.SelectedIndex = 0;
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            var form = new RequestForm();
            if (form.ShowDialog() == true)
            {
                _requests.Add(form.Request);
                RefreshData();
            }
        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsGrid.SelectedItem is ServiceRequest selectedRequest)
            {
                var form = new RequestForm(selectedRequest);
                if (form.ShowDialog() == true)
                {
                    RefreshData();
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsGrid.SelectedItem is ServiceRequest selectedRequest)
            {
                var result = MessageBox.Show("Удалить выбранную заявку?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _requests.Remove(selectedRequest);
                    RefreshData();
                }
            }
        }

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statsForm = new StatisticsForm(_requests);
            statsForm.ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            _filteredRequests = _requests.Where(r =>
                (string.IsNullOrEmpty(SearchTextBox.Text) ||
                 r.RequestNumber.Contains(SearchTextBox.Text) ||
                 r.ClientName.Contains(SearchTextBox.Text) ||
                 r.EquipmentType.Contains(SearchTextBox.Text)) &&
                (StatusFilterComboBox.SelectedIndex == 0 ||
                 r.Status.ToString() == StatusFilterComboBox.SelectedItem.ToString().Replace("ая", "").Replace("а", "").Trim())
            ).ToList();

            RequestsGrid.ItemsSource = _filteredRequests;
        }

        private void RefreshData()
        {
            ApplyFilters();
        }
    }
}