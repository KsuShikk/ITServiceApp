using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;
using ITServiceApp.UI;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITServiceApp.UI
{
    public partial class ServiceRequestEditWindow : Window
    {
        public ServiceRequest ServiceRequest { get; set; }
        private readonly IEngineerRepository _engineerRepository;

        public ObservableCollection<Engineer> AvailableEngineers { get; set; }
        public static ObservableCollection<string> AvailableEquipmentTypes { get; set; }
        public static Array AvailableStatuses { get; private set; }

        static ServiceRequestEditWindow()
        {
            AvailableEquipmentTypes = new ObservableCollection<string>
            {
                "Компьютер", "Сервер", "Принтер", "Монитор", "Ноутбук",
                "Сетевое оборудование", "Сканер", "ИБП", "МФУ",
                "Планшет", "Смартфон", "Проектор"
            };
            AvailableStatuses = Enum.GetValues(typeof(RequestStatus));
        }

        public ServiceRequestEditWindow(ServiceRequest serviceRequest, IEngineerRepository engineerRepository)
        {
            InitializeComponent();
            _engineerRepository = engineerRepository;

            var engineers = _engineerRepository.GetAll();
            AvailableEngineers = new ObservableCollection<Engineer>(engineers);

            ServiceRequest = serviceRequest != null ? serviceRequest.Clone() : ServiceRequest.Create();
            DataContext = this;
        }

        public static ServiceRequest Create(IEngineerRepository engineerRepository)
        {
            var window = new ServiceRequestEditWindow(null, engineerRepository);
            window.Title = "Создание заявки";
            var result = window.ShowDialog();
            return result == true ? window.ServiceRequest : null;
        }

        public static ServiceRequest Edit(ServiceRequest serviceRequest, IEngineerRepository engineerRepository)
        {
            var window = new ServiceRequestEditWindow(serviceRequest, engineerRepository);
            window.Title = "Редактирование заявки";
            var result = window.ShowDialog();
            return result == true ? window.ServiceRequest : null;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(out string validationError))
            {
                MessageBox.Show(validationError, "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private bool ValidateForm(out string validationError)
        {
            validationError = string.Empty;

            if (string.IsNullOrWhiteSpace(ServiceRequest.RequestNumber))
            {
                validationError = "Укажите номер заявки";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ServiceRequest.Client.Name))
            {
                validationError = "Укажите имя клиента";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ServiceRequest.Client.Phone))
            {
                validationError = "Укажите телефон клиента";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ServiceRequest.EquipmentType))
            {
                validationError = "Выберите тип оборудования";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ServiceRequest.EquipmentModel))
            {
                validationError = "Укажите модель оборудования";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ServiceRequest.ProblemDescription))
            {
                validationError = "Укажите описание проблемы";
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}