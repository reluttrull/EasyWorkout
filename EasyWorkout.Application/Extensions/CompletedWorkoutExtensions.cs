using EasyWorkout.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static EasyWorkout.Contracts.Model.Enums;

namespace EasyWorkout.Application.Extensions
{
    public static class CompletedWorkoutExtensions
    {
        private static Dictionary<WeightUnit, double> _conversionFactors = new Dictionary<WeightUnit, double>()
        {
            { WeightUnit.Pounds, 1.0 },
            { WeightUnit.Ounces, 0.0625 },
            { WeightUnit.Kilograms, 2.20462 },
            { WeightUnit.Grams, 0.00220462 },
            { WeightUnit.Stones, 14 }
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
                        setVolume *= _conversionFactors[set.WeightUnit.Value];
                        setVolume = setVolume / _conversionFactors[targetUnit];
                        totalVolume += setVolume;
                    }
                }
            }
            return totalVolume;
        }
    }
}
