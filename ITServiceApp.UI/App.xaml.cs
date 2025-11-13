using System;
using System.Collections.Generic;
using System.Windows;
using ITServiceApp.Data.InMemory;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.UI
{
    public partial class App : Application
    {
        private IServiceRequestRepository _requestRepository;
        private IEngineerRepository _engineerRepository;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _requestRepository = new ServiceRequestRepository();
            _engineerRepository = new EngineerRepository();

            SeedInitialData();

            var mainWindow = new ServiceRequestsListWindow(_requestRepository, _engineerRepository);
            mainWindow.Show();
        }

        private void SeedInitialData()
        {
            var engineers = new List<Engineer>
            {
                new Engineer { Id = Guid.NewGuid(), Name = "Сергеев А.В.", Specialization = "Компьютеры и серверы"},
                new Engineer { Id = Guid.NewGuid(), Name = "Петрова И.С.", Specialization = "Оргтехника"},
                new Engineer { Id = Guid.NewGuid(), Name = "Козлов В.П.", Specialization = "Сетевое оборудование"},
                new Engineer { Id = Guid.NewGuid(), Name = "Николаева М.К.", Specialization = "Мобильные устройства"}
            };

            var engineerRepo = (EngineerRepository)_engineerRepository;
            engineerRepo.Seed(engineers);

            _requestRepository.Add(new ServiceRequest
            {
                Id = Guid.Empty,
                Client = new Client { Name = "Иванов Иван", Phone = "+7(999)123-45-67"},
                Engineer = engineers[0],
                EquipmentType = "Компьютер",
                EquipmentModel = "Dell Optiplex 7070",
                ProblemDescription = "Не включается, нет реакции на кнопку питания",
                CreationDate = DateTime.Now.AddDays(-2),
                Status = RequestStatus.InProgress,
                Comments = "Проверить блок питания",
                OrderedParts = "Блок питания 500W"
            });

            _requestRepository.Add(new ServiceRequest
            {
                Id = Guid.Empty,
                Client = new Client { Name = "Петров Петр", Phone = "+7(999)765-43-21"},
                Engineer = engineers[1],
                EquipmentType = "Принтер",
                EquipmentModel = "HP LaserJet Pro",
                ProblemDescription = "Зажевывает бумагу при печати",
                CreationDate = DateTime.Now.AddDays(-1),
                Status = RequestStatus.Created,
                Comments = "Требуется диагностика",
                OrderedParts = ""
            });

            // ДОБАВЛЯЕМ тестовые данные для статистики
            var random = new Random();
            var statuses = new[]
            {
        RequestStatus.Created,
        RequestStatus.InProgress,
        RequestStatus.WaitingForParts,
        RequestStatus.Completed
    };

            var equipmentTypes = new[]
            {
        "Компьютер", "Сервер", "Принтер", "Монитор", "Ноутбук",
        "Сетевое оборудование", "Сканер", "ИБП", "МФУ"
    };

            var clientNames = new[]
            {
        "Сидорова Анна", "Кузнецов Алексей", "Смирнова Мария", "Попов Дмитрий",
        "Васильева Елена", "Новиков Сергей", "Федоров Игорь", "Орлова Ольга"
    };

            // Генерируем 30 дополнительных тестовых заявок
            for (int i = 0; i < 30; i++)
            {
                var request = new ServiceRequest
                {
                    Id = Guid.Empty,
                    Client = new Client
                    {
                        Name = clientNames[random.Next(clientNames.Length)],
                        Phone = $"+7(999){random.Next(100, 999)}-{random.Next(10, 99)}-{random.Next(10, 99)}"
                    },
                    Engineer = engineers[random.Next(engineers.Count)],
                    EquipmentType = equipmentTypes[random.Next(equipmentTypes.Length)],
                    EquipmentModel = $"Model {random.Next(1000, 9999)}",
                    ProblemDescription = $"Тестовое описание проблемы {i + 1}",
                    CreationDate = DateTime.Now.AddDays(-random.Next(0, 180)),
                    Status = statuses[random.Next(statuses.Length)],
                    Comments = $"Тестовый комментарий {i + 1}",
                    OrderedParts = random.Next(0, 3) == 0 ? $"Запчасть {random.Next(1, 10)}" : ""
                };
                _requestRepository.Add(request);
            }
        }
    }
}