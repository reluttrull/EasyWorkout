import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExercisesService } from './exercises.service';
import { Exercise } from '../model/interfaces';
import { ExerciseComponent } from '../components/exercise/exercise';
import { CreateExercise } from '../components/create-exercise/create-exercise';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  standalone: true,
  imports: [CommonModule, ExerciseComponent, CreateExercise, OrderByPipe],
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

  showAddExercise() {
    this.isCreateVisible = true;
  }

  handleCancelCreate() {
    this.isCreateVisible = false;
    this.reload();
  }
}