using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Responses
{
    public class AveragePercentCompletedReportResponse
    {
        public List<DataPointResponse> DataPoints { get; set; } = [];
    }
}
