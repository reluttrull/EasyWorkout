using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Responses
{
    public class TotalVolumeReportResponse
    {
        public WeightUnit WeightUnit { get; set; }
        public List<DataPointResponse> DataPoints { get; set; } = [];
    }
}
