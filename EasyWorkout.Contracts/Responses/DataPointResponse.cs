namespace EasyWorkout.Contracts.Responses
{
    public record DataPointResponse(Guid completedWorkoutId, DateTime completedDate, double totalVolume);
}
