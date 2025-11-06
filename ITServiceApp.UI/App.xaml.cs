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
        }
    }
}