import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CompletedWorkoutsService } from './completed-workouts.service';
import { CompletedWorkoutResponse } from '../model/interfaces';
import { CompletedWorkoutComponent } from '../components/completed-workout/completed-workout';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, CompletedWorkoutComponent, OrderByPipe],
  templateUrl: './completed-workouts.html',
  styleUrl: './completed-workouts.css'
})
export class CompletedWorkoutsComponent {
  completedWorkouts = signal<CompletedWorkoutResponse[]>([]);
  isLoaded = signal(false);
  isCreateVisible = false;

  constructor(private completedWorkoutsService: CompletedWorkoutsService) {
    this.reload();
  }

  reload() {
    this.completedWorkoutsService.getAll().subscribe(w => {
      this.completedWorkouts.set(w);
      this.isLoaded.set(true);
    });
  }

  showAddWorkout() {
    this.isCreateVisible = true;
  }

  handleCancelCreate() {
    this.isCreateVisible = false;
    this.reload();
  }
}