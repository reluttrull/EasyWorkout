using EasyWorkout.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Application.Extensions
{
    public static class CompletedWorkoutExtensions
    {
        private static Dictionary<WeightUnit, double> _weightConversionFactors = new Dictionary<WeightUnit, double>()
        {
            { WeightUnit.Pounds, 1.0 },
            { WeightUnit.Ounces, 0.0625 },
            { WeightUnit.Kilograms, 2.20462 },
            { WeightUnit.Grams, 0.00220462 },
            { WeightUnit.Stones, 14 }
        };
        private static Dictionary<DurationUnit, double> _durationConversionFactors = new Dictionary<DurationUnit, double>()
        {
            { DurationUnit.Minutes, 1.0 },
            { DurationUnit.Hours, 60.0 },
            { DurationUnit.Seconds, 0.016667 }
        };
        private static Dictionary<DistanceUnit, double> _distanceConversionFactors = new Dictionary<DistanceUnit, double>()
        {
            { DistanceUnit.Miles, 1.0 },
            { DistanceUnit.Feet, 0.000189 },
            { DistanceUnit.Yards, 0.000568 },
            { DistanceUnit.Laps, 0.248548 },
            { DistanceUnit.Meters, 0.000621 },
            { DistanceUnit.Kilometers, 0.621371 }
        };
        public static double GetTotalVolume(this CompletedWorkout cw, WeightUnit targetUnit)
        {
            double totalVolume = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Reps: not null, Weight: not null, WeightUnit: not null })
                    {
                        double setVolume = set.Reps.Value * set.Weight.Value;
                        setVolume *= _weightConversionFactors[set.WeightUnit.Value];
                        setVolume = setVolume / _weightConversionFactors[targetUnit];
                        totalVolume += setVolume;
                    }
                }
            }
            return totalVolume;
        }

        public static double GetTotalTime(this CompletedWorkout cw, DurationUnit targetUnit)
        {
            double totalTime = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Duration: not null, DurationUnit: not null })
                    {
                        double setTime = set.Duration.Value;
                        setTime *= _durationConversionFactors[set.DurationUnit.Value];
                        setTime = setTime / _durationConversionFactors[targetUnit];
                        totalTime += setTime;
                    }
                }
            }
            return totalTime;
        }

        public static double GetTotalDistance(this CompletedWorkout cw, DistanceUnit targetUnit)
        {
            double totalDistance = 0;
            foreach (CompletedExercise ex in cw.CompletedExercises)
            {
                foreach (CompletedExerciseSet set in ex.CompletedExerciseSets)
                {
                    if (set is { Distance: not null, DistanceUnit: not null })
                    {
                        double setDistance = set.Distance.Value;
                        setDistance *= _distanceConversionFactors[set.DistanceUnit.Value];
                        setDistance = setDistance / _distanceConversionFactors[targetUnit];
                        totalDistance += setDistance;
                    }
                }
            }
            return totalDistance;
        }
    }
}
