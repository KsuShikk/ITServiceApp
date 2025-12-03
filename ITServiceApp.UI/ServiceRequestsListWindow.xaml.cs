using ITServiceApp.Data.Interfaces;
using ITServiceApp.Data.SqlServer;
using ITServiceApp.Domain.Models;
using ITServiceApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ITServiceApp.UI
{
    public partial class ServiceRequestsListWindow : Window
    {
        private readonly IServiceRequestRepository _requestRepository;
        private readonly IEngineerRepository _engineerRepository;

        public ObservableCollection<ServiceRequest> ServiceRequests { get; set; }
        public ServiceRequest SelectedServiceRequest { get; set; }

        public ServiceRequestsListWindow(IServiceRequestRepository requestRepository, IEngineerRepository engineerRepository)
        {
            InitializeComponent();
            _requestRepository = requestRepository;
            _engineerRepository = engineerRepository; // если у тебя другое имя поля — оставь свое
            ServiceRequests = new ObservableCollection<ServiceRequest>();
            DataContext = this;
            LoadServiceRequests();
        }

        private void LoadServiceRequests()
        {
            var requests = _requestRepository.GetAll(ServiceRequestFilter.Empty);
            ServiceRequests.Clear();
            foreach (var request in requests)
            {
                ServiceRequests.Add(request);
            }
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = ServiceRequestEditWindow.Create(_engineerRepository);
            if (newRequest != null)
            {
                _requestRepository.Add(newRequest);
                LoadServiceRequests();
                MessageBox.Show("Заявка успешно добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = SelectedServiceRequest;
            if (selectedRequest != null)
            {
                var updatedRequest = ServiceRequestEditWindow.Edit(selectedRequest, _engineerRepository);
                if (updatedRequest != null)
                {
                    // передаём в Update id и именно обновлённый объект
                    _requestRepository.Update(selectedRequest.Id, updatedRequest);
                    LoadServiceRequests();
                    MessageBox.Show("Заявка успешно обновлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
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
            var selectedRequest = SelectedServiceRequest;
            if (selectedRequest != null)
            {
                var result = MessageBox.Show(
                    $"Удалить заявку {selectedRequest.RequestNumber}?\nКлиент: {selectedRequest.Client.Name}",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // передаём в Delete идентификатор, а не весь объект
                    _requestRepository.Delete(selectedRequest.Id);
                    LoadServiceRequests();
                    MessageBox.Show("Заявка успешно удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadServiceRequests();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            var statisticsService = new StatisticsService(_requestRepository);
            var window = new StatisticsWindow(statisticsService);
            window.ShowDialog();
        }
    }
}
