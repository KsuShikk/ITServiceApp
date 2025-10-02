using ITServiceApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ITServiceApp.UI
{
    public partial class StatisticsForm : Window
    {
        private List<ServiceRequest> _requests;

        public StatisticsForm(List<ServiceRequest> requests)
        {
            InitializeComponent();
            _requests = requests;
            ShowStatistics();
        }

        private void ShowStatistics()
        {
            var stats = new StringBuilder();

            // Общее количество заявок
            stats.AppendLine($"Всего заявок: {_requests.Count}");

            // По статусам
            stats.AppendLine($"Новых: {_requests.Count(r => r.Status == RequestStatus.New)}");
            stats.AppendLine($"В работе: {_requests.Count(r => r.Status == RequestStatus.InProgress)}");
            stats.AppendLine($"Завершено: {_requests.Count(r => r.Status == RequestStatus.Completed)}");

            // Популярные типы оборудования
            var popularType = _requests
                .GroupBy(r => r.EquipmentType)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (popularType != null)
            {
                stats.AppendLine($"Чаще всего: {popularType.Key}");
            }

            StatsText.Text = stats.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}