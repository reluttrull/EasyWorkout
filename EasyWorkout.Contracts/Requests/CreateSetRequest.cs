using EasyWorkout.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EasyWorkout.Contracts.Requests
{
    public class CreateSetRequest
    {
        public int? Reps { get; set; }
        public double? Weight { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Enums.WeightUnit? WeightUnit {  get; set; }
        public double? Duration { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Enums.DurationUnit? DurationUnit { get; set; }
        public string? Notes { get; set; }
    }
}
