import { Component, input } from '@angular/core';
import { Workout } from '../../model/interfaces';
import { ExerciseComponent } from '../exercise/exercise';

@Component({
  selector: 'app-workout',
  imports: [ExerciseComponent],
  templateUrl: './workout.html',
  styleUrl: './workout.css',
})
export class WorkoutComponent {
  workout = input.required<Workout>();
  workoutDetail = false;

  toggleWorkoutDetail() {
    this.workoutDetail = !this.workoutDetail;
  }
}
