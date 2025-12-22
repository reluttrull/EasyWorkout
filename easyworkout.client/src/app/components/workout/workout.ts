import { Component, input, output } from '@angular/core';
import { Workout } from '../../model/interfaces';
import { ExerciseComponent } from '../exercise/exercise';
import { WorkoutsService } from '../../workouts/workouts.service';

@Component({
  selector: 'app-workout',
  imports: [ExerciseComponent],
  templateUrl: './workout.html',
  styleUrl: './workout.css',
})
export class WorkoutComponent {
  workout = input.required<Workout>();
  onWorkoutChanged = output();
  workoutDetail = false;

  constructor(private workoutsService: WorkoutsService) {}

  toggleWorkoutDetail() {
    this.workoutDetail = !this.workoutDetail;
  }

  delete() {
  if(confirm("Are you sure you want to delete workout " + this.workout().name + "?")) {
    // Implement delete/action functionality here if confirmed
    console.log('id', this.workout().id);
    this.workoutsService.delete(this.workout().id)
      .subscribe({
        next: () => {
          this.onWorkoutChanged.emit();
        },
        error: err => console.error(err)
      });
    console.log("Item deleted");
  } else {
    console.log("Deletion cancelled");
  }
}
}
