namespace ITServiceApp.Data.Interfaces;

public record ServiceRequestFilter
{
    public static ServiceRequestFilter Empty => new();

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
