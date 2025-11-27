using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ITServiceApp.UI
{
    public partial class StatisticsWindow : Window, INotifyPropertyChanged
    {
        private readonly StatisticsService _statisticsService;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public PlotModel? StatusPlotModel { get; set; }
        public PlotModel? EngineerPlotModel { get; set; }
        public PlotModel? MonthPlotModel { get; set; }

        public StatisticsWindow(StatisticsService statisticsService)
        {
            InitializeComponent();
            _statisticsService = statisticsService;
            DataContext = this;

            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var filter = new ServiceRequestFilter
            {
                StartDate = StartDate,
                EndDate = EndDate
            };

            LoadStatusChart(filter);
            LoadEngineerChart(filter);
            LoadMonthChart(filter);
        }

        private void LoadStatusChart(ServiceRequestFilter filter)
        {
            try
            {
                var data = _statisticsService.GetRequestsByStatus(filter);

                var plotModel = new PlotModel { Title = "Распределение заявок по статусам" };

                var pieSeries = new PieSeries
                {
                    StrokeThickness = 2.0,
                    InsideLabelPosition = 0.5,
                    AngleSpan = 360,
                    StartAngle = 0
                };

                foreach (var item in data)
                {
                    var label = GetStatusText(item.Status);
                    pieSeries.Slices.Add(new PieSlice(label, item.Count)
                    {
                        IsExploded = false
                    });
                }

                plotModel.Series.Add(pieSeries);
                StatusPlotModel = plotModel;
                OnPropertyChanged(nameof(StatusPlotModel));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки диаграммы статусов: {ex.Message}");
            }
        }

        private void LoadEngineerChart(ServiceRequestFilter filter)
        {
            try
            {
                var data = _statisticsService.GetRequestsByEngineer(filter);

                var plotModel = new PlotModel { Title = "Распределение заявок по инженерам" };

                var categoryAxis = new CategoryAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Инженеры"
                };

                foreach (var item in data)
                {
                    categoryAxis.Labels.Add(item.EngineerName);
                }

                plotModel.Axes.Add(categoryAxis);

                plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Количество заявок",
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var barSeries = new BarSeries
                {
                    Title = "Количество заявок",
                    FillColor = OxyColor.FromRgb(79, 129, 189)
                };

                foreach (var item in data)
                {
                    barSeries.Items.Add(new BarItem { Value = item.Count });
                }

                plotModel.Series.Add(barSeries);
                EngineerPlotModel = plotModel;
                OnPropertyChanged(nameof(EngineerPlotModel));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки диаграммы инженеров: {ex.Message}");
            }
        }

        private void LoadMonthChart(ServiceRequestFilter filter)
        {
            try
            {
                var data = _statisticsService.GetRequestsByMonth(filter);

                var plotModel = new PlotModel { Title = "Динамика заявок по месяцам" };

                var categoryAxis = new CategoryAxis
                {
                    Position = AxisPosition.Bottom,
                    Angle = -15,
                    Title = "Месяцы"
                };

                foreach (var item in data)
                {
                    categoryAxis.Labels.Add(item.GetMonthName());
                }

                plotModel.Axes.Add(categoryAxis);

                plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Количество заявок",
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var lineSeries = new LineSeries
                {
                    Title = "Количество заявок",
                    Color = OxyColor.FromRgb(79, 129, 189),
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4,
                    MarkerFill = OxyColor.FromRgb(79, 129, 189)
                };

                for (int i = 0; i < data.Count; i++)
                {
                    lineSeries.Points.Add(new DataPoint(i, data[i].Count));
                }

                plotModel.Series.Add(lineSeries);
                MonthPlotModel = plotModel;
                OnPropertyChanged(nameof(MonthPlotModel));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки диаграммы месяцев: {ex.Message}");
            }
        }

        private string GetStatusText(Domain.Models.RequestStatus status)
        {
            switch (status)
            {
                case Domain.Models.RequestStatus.Created:
                    return "Создана";
                case Domain.Models.RequestStatus.InProgress:
                    return "В процессе";
                case Domain.Models.RequestStatus.WaitingForParts:
                    return "Ожидание запчастей";
                case Domain.Models.RequestStatus.Completed:
                    return "Завершена";
                default:
                    return status.ToString();
            }
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = null;
            EndDate = null;
            LoadStatistics();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}