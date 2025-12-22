import { Component, input, output } from '@angular/core';
import { Exercise } from '../../model/interfaces';
import { SetComponent } from '../set/set';

@Component({
  selector: 'app-exercise',
  imports: [SetComponent],
  templateUrl: './exercise.html',
  styleUrl: './exercise.css',
})
export class ExerciseComponent {
  exercise = input.required<Exercise>();
  onExerciseChanged = output();
  exerciseDetail = false;

  toggleExerciseDetail() {
    this.exerciseDetail = !this.exerciseDetail;
  }
}
