using ITServiceApp.Domain.Models;

namespace ITServiceApp.Domain.Statistics;

public record StatusStatisticItem
{
    public required RequestStatus Status { get; set; }
    public required int Count { get; set; }
}