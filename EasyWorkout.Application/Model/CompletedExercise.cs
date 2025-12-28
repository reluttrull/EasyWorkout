using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyWorkout.Application.Model
{
    public class CompletedExercise
    {
        [Key]
        public required Guid Id { get; init; }
        public Guid ExerciseId { get; init; }
        public required Guid CompletedByUserId { get; init; }
        public required DateTime CompletedDate { get; init; }
        [Required]
        [MaxLength(75)]
        public string FallbackName { get; set; } = string.Empty;
        public string Name => Exercise?.Name ?? FallbackName; // existing exercise name, or fallback name if deleted
        [MaxLength(250)]
        public string? CompletedNotes { get; set; }
        public string OriginalNotes => Exercise?.Notes ?? string.Empty;
        public virtual Exercise? Exercise { get; set; }
        public List<CompletedExerciseSet> CompletedExerciseSets { get; init; } = [];
    }
}
