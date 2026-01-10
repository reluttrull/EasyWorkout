using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Responses
{
    public class TotalTimeReportResponse
    {
        public DurationUnit DurationUnit { get; set; }
        public List<DataPointResponse> DataPoints { get; set; } = [];
    }
}
