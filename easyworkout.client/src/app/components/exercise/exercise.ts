import { Component, input } from '@angular/core';
import { Exercise } from '../../model/interfaces';

@Component({
  selector: 'app-exercise',
  imports: [],
  templateUrl: './exercise.html',
  styleUrl: './exercise.css',
})
export class ExerciseComponent {
  exercise = input.required<Exercise>();
}
