using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;

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
            _engineerRepository = engineerRepository;
            ServiceRequests = new ObservableCollection<ServiceRequest>();
            DataContext = this;
            LoadServiceRequests();
        }

        private void LoadServiceRequests()
        {
            var requests = _requestRepository.GetAll();
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
                    _requestRepository.Update(updatedRequest);
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
                    _requestRepository.Delete(selectedRequest);
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

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            var requests = _requestRepository.GetAll();
            var totalRequests = requests.Count;
            var completedRequests = requests.Count(r => r.Status == RequestStatus.Completed);
            var inProgressRequests = requests.Count(r => r.Status == RequestStatus.InProgress);
            var waitingRequests = requests.Count(r => r.Status == RequestStatus.WaitingForParts);
            var newRequests = requests.Count(r => r.Status == RequestStatus.Created);

            MessageBox.Show(
                $"Статистика заявок:\n\n" +
                $"Всего заявок: {totalRequests}\n" +
                $"Завершено: {completedRequests}\n" +
                $"В процессе: {inProgressRequests}\n" +
                $"Ожидание запчастей: {waitingRequests}\n" +
                $"Новых: {newRequests}",
                "Статистика",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}