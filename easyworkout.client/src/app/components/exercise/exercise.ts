import { Component, input, output } from '@angular/core';
import { WorkoutsService } from '../../workouts/workouts.service';
import { ExercisesService } from '../../exercises/exercises.service';
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
  workoutId = input.required<string>();
  onExerciseChanged = output();
  exerciseDetail = false;

  constructor(private workoutsService: WorkoutsService, private exercisesService: ExercisesService) {
    
  }

  toggleExerciseDetail() {
    this.exerciseDetail = !this.exerciseDetail;
  }  
  
  remove(exerciseId:string) {
    this.workoutsService.removeExercise(this.workoutId(), exerciseId).subscribe({
      next: () => {
        this.onExerciseChanged.emit();
      },
      error: err => console.error(err)
    });
  }
  
  delete(exerciseId:string) {
    this.exercisesService.delete(exerciseId).subscribe({
      next: () => {
        this.onExerciseChanged.emit();
      },
      error: err => console.error(err)
    });
  }
}
