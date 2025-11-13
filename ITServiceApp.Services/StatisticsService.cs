using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Statistics;

namespace ITServiceApp.Services;

public class StatisticsService
{
    private readonly IServiceRequestRepository _requestRepository;

    public StatisticsService(IServiceRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public List<StatusStatisticItem> GetRequestsByStatus(ServiceRequestFilter filter)
    {
        var requests = _requestRepository.GetAll(filter);

        return requests
            .GroupBy(r => r.Status)
            .Select(g => new StatusStatisticItem
            {
                Status = g.Key,
                Count = g.Count()
            })
            .OrderBy(s => s.Status)
            .ToList();
    }

    public List<MonthStatisticItem> GetRequestsByMonth(ServiceRequestFilter filter)
    {
        var requests = _requestRepository.GetAll(filter);

        return requests
            .GroupBy(r => new { r.CreationDate.Year, r.CreationDate.Month })
            .Select(g => new MonthStatisticItem
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();
    }

    public List<EngineerStatisticItem> GetRequestsByEngineer(ServiceRequestFilter filter)
    {
        var requests = _requestRepository.GetAll(filter);

        return requests
            .Where(r => r.Engineer != null)
            .GroupBy(r => r.Engineer!.Name)
            .Select(g => new EngineerStatisticItem
            {
                EngineerName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(e => e.Count)
            .ToList();
    }
}
