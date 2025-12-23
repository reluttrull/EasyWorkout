import { Component, input } from '@angular/core';
import { Exercise } from '../../model/interfaces';

@Component({
  selector: 'app-exercise-brief',
  imports: [],
  templateUrl: './exercise-brief.html',
  styleUrl: './exercise-brief.css',
})
export class ExerciseBrief {
  exercise = input.required<Exercise>();
}
