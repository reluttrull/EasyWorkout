import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ExercisesService } from './exercises.service';
import { ExerciseResponse } from '../model/interfaces';
import { ExerciseComponent } from '../components/exercise/exercise';
import { CreateExercise } from '../components/create-exercise/create-exercise';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, ExerciseComponent, CreateExercise, OrderByPipe],
  templateUrl: './exercises.html',
  styleUrl: './exercises.css'
})
export class ExercisesComponent {
  exercises = signal<ExerciseResponse[]>([]);
  isLoaded = signal(false);
  isCreateVisible = false;

  constructor(private service: ExercisesService) {
    this.reload();
  }

  reload() {
    this.service.getAll().subscribe(e => {
      this.exercises.set(e);
      this.isLoaded.set(true);
    });
  }

  showAddExercise() {
    this.isCreateVisible = true;
  }

  handleCancelCreate() {
    this.isCreateVisible = false;
    this.reload();
  }
}