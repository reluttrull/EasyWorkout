using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Contracts.Requests
{
    public class TotalTimeReportRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public Guid? WorkoutId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DurationUnit DurationUnit { get; set; }
    }
}
