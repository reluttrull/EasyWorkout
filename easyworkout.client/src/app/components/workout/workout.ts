import { Component, input } from '@angular/core';
import { Workout } from '../../model/interfaces';

@Component({
  selector: 'app-workout',
  imports: [],
  templateUrl: './workout.html',
  styleUrl: './workout.css',
})
export class WorkoutComponent {
  workout = input.required<Workout>();
  detail = input<boolean>(false);
}
