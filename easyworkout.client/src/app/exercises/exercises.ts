import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExercisesService } from './exercises.service';
import { Exercise } from '../model/interfaces';
import { ExerciseComponent } from '../components/exercise/exercise';
import { CreateExercise } from '../components/create-exercise/create-exercise';

@Component({
  standalone: true,
  imports: [CommonModule, ExerciseComponent, CreateExercise],
  templateUrl: './exercises.html',
  styleUrl: './exercises.css'
})
export class ExercisesComponent {
  exercises = signal<Exercise[]>([]);
  isCreateVisible = false;

  constructor(private service: ExercisesService) {
    this.service.getAll().subscribe(e => this.exercises.set(e));
  }

  reload() {
    this.service.getAll().subscribe(e => this.exercises.set(e));
  }

  showAddWorkout() {
    this.isCreateVisible = true;
  }

  handleCancelCreate() {
    this.isCreateVisible = false;
    this.reload();
  }
}