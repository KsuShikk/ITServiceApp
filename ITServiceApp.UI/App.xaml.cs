using ITServiceApp.Data.Interfaces;
using ITServiceApp.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace ITServiceApp.UI
{
    public partial class App : Application
    {
        private ITServiceAppDbContext? _dbContext;
        private IServiceRequestRepository? _requestRepository;
        private IEngineerRepository? _engineerRepository;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            var factory = new ITServiceAppDbContextFactory();
            _dbContext = factory.CreateDbContext(configuration);

            _dbContext.Database.Migrate();

            _requestRepository = new ServiceRequestRepository(_dbContext);
            _engineerRepository = new EngineerRepository(_dbContext);

            SeedInitData();

            var mainWindow = new ServiceRequestsListWindow(_requestRepository, _engineerRepository);
            mainWindow.Show();
        }

        private void SeedInitData()
        {
            if (_engineerRepository == null || _requestRepository == null) return;

            if (_engineerRepository.GetAll().Any()) return;

            var eng1 = new ITServiceApp.Domain.Models.Engineer { Id = Guid.NewGuid(), Name = "Иван Иванов", Specialization = "Сеть" };
            _engineerRepository.Add(eng1);
            // ... добавить тестовые заявки аналогично
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }
}
