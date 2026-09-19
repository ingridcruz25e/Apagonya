using ApagonYa.Api.Models;
namespace ApagonYa.Api.Services;
public class StatisticsService { public Task<IReadOnlyList<ZoneStatistics>> GetStatisticsAsync() { IReadOnlyList<ZoneStatistics> result = Array.Empty<ZoneStatistics>(); return Task.FromResult(result); } }
