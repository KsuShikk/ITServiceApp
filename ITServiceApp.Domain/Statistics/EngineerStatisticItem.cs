namespace ITServiceApp.Domain.Statistics;

public record EngineerStatisticItem
{
    public required string EngineerName { get; set; }
    public required int Count { get; set; }
}
