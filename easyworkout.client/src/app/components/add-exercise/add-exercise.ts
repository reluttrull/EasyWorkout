import { Component, input, output, signal, inject } from '@angular/core';
import { Exercise } from '../../model/interfaces';
import { WorkoutsService } from '../../workouts/workouts.service';
import { ExercisesService } from '../../exercises/exercises.service';
import { ExerciseBrief } from '../exercise-brief/exercise-brief';
import { CreateExercise } from '../create-exercise/create-exercise';
import { OrderByPipe } from '../../pipes/order-by-pipe';

@Component({
  selector: 'app-add-exercise',
  imports: [ExerciseBrief, CreateExercise, OrderByPipe],
  templateUrl: './add-exercise.html',
  styleUrl: './add-exercise.css',
})
export class AddExercise {
  workoutsService = inject(WorkoutsService);
  exercises = signal<Exercise[]>([]);
  workoutId = input.required<string>();
  onClose = output();
  isCreateVisible = false;
  
  constructor(private exercisesService: ExercisesService) {
    this.loadExercises();
  }

  add(exerciseId:string) {
    this.workoutsService.addExercise(this.workoutId(), exerciseId).subscribe({
      next: () => {
        this.onClose.emit();
      },
      error: err => console.error(err)
    });
  }

  close() {
    this.onClose.emit();
  }

  closeCreate() {
    this.loadExercises();
    this.isCreateVisible = false;
  }

  loadExercises() {
    this.exercisesService.getAll().subscribe(e => this.exercises.set(e));
  }

  toggleIsCreateVisible() {
    this.isCreateVisible = !this.isCreateVisible;
  }
}
