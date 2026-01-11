using EasyWorkout.Application.Model;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Application.Extensions
{
    public static class CompletedWorkoutExtensions
    {
        private static Dictionary<WeightUnit, decimal> _weightConversionFactors = new Dictionary<WeightUnit, decimal>()
        {
            { WeightUnit.Pounds, 1.0m },
            { WeightUnit.Ounces, 0.0625m },
            { WeightUnit.Kilograms, 2.20462m },
            { WeightUnit.Grams, 0.00220462m },
            { WeightUnit.Stones, 14m }
        };
        private static Dictionary<DurationUnit, decimal> _durationConversionFactors = new Dictionary<DurationUnit, decimal>()
        {
            { DurationUnit.Minutes, 1.0m },
            { DurationUnit.Hours, 60.0m },
            { DurationUnit.Seconds, 0.016667m }
        };
        private static Dictionary<DistanceUnit, decimal> _distanceConversionFactors = new Dictionary<DistanceUnit, decimal>()
        {
            { DistanceUnit.Miles, 1.0m },
            { DistanceUnit.Feet, 0.000189m },
            { DistanceUnit.Yards, 0.000568m },
            { DistanceUnit.Laps, 0.248548m },
            { DistanceUnit.Meters, 0.000621371m },
            { DistanceUnit.Kilometers, 0.621371m }
        };
        public static decimal GetTotalVolume(this CompletedWorkout cw, WeightUnit targetUnit)
        {
            decimal totalVolume = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Reps: not null, Weight: not null, WeightUnit: not null })
                    {
                        decimal setVolume = (decimal)set.Reps.Value * (decimal)set.Weight.Value;
                        setVolume *= _weightConversionFactors[set.WeightUnit.Value];
                        setVolume = setVolume / _weightConversionFactors[targetUnit];
                        totalVolume += setVolume;
                    }
                }
            }
            return Math.Round(totalVolume, 4);
        }

        public static decimal GetTotalTime(this CompletedWorkout cw, DurationUnit targetUnit)
        {
            decimal totalTime = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Duration: not null, DurationUnit: not null })
                    {
                        decimal setTime = (decimal)set.Duration.Value;
                        setTime *= _durationConversionFactors[set.DurationUnit.Value];
                        setTime = setTime / _durationConversionFactors[targetUnit];
                        totalTime += setTime;
                    }
                }
            }
            return Math.Round(totalTime, 4);
        }

        public static decimal GetTotalDistance(this CompletedWorkout cw, DistanceUnit targetUnit)
        {
            decimal totalDistance = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Distance: not null, DistanceUnit: not null })
                    {
                        decimal setDistance = (decimal)set.Distance.Value;
                        setDistance *= _distanceConversionFactors[set.DistanceUnit.Value];
                        setDistance = setDistance / _distanceConversionFactors[targetUnit];
                        totalDistance += setDistance;
                    }
                }
            }
            return Math.Round(totalDistance, 4);
        }

        public static decimal GetAveragePercentCompleted(this CompletedWorkout cw)
        {
            List<decimal> percentages = [];
            foreach (CompletedExerciseSet set in cw.CompletedExercises.SelectMany(ce => ce.CompletedExerciseSets))
            {
                if (set.GoalReps is not null && set.GoalReps != 0)
                {
                    percentages.Add((decimal)(set.Reps ?? 0) / set.GoalReps.Value);
                }
                if (set.GoalWeight is not null && set.GoalWeight != 0)
                {
                    percentages.Add((decimal)(set.Weight ?? 0) / (decimal)set.GoalWeight.Value);
                }
                if (set.GoalDuration is not null && set.GoalDuration != 0)
                {
                    percentages.Add((decimal)(set.Duration ?? 0) / (decimal)set.GoalDuration.Value);
                }
                if (set.GoalDistance is not null && set.GoalDistance != 0)
                {
                    percentages.Add((decimal)(set.Distance ?? 0) / (decimal)set.GoalDistance.Value);
                }
            }
            return percentages.Count > 0 ? Math.Round(percentages.Average() * 100, 4) : 100;
        }
    }
}
