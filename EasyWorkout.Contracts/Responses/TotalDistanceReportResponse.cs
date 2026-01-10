using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Responses
{
    public class TotalDistanceReportResponse
    {
        public DistanceUnit DistanceUnit { get; set; }
        public List<DataPointResponse> DataPoints { get; set; } = [];
    }
}
