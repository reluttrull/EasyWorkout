import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompletedWorkoutsService } from './completed-workouts.service';
import { CompletedWorkoutResponse } from '../model/interfaces';
import { CompletedWorkoutComponent } from '../components/completed-workout/completed-workout';
import { CreateWorkout } from '../components/create-workout/create-workout';

@Component({
  standalone: true,
  imports: [CommonModule, CompletedWorkoutComponent],
  templateUrl: './completed-workouts.html',
  styleUrl: './completed-workouts.css'
})
export class CompletedWorkoutsComponent {
  completedWorkouts = signal<CompletedWorkoutResponse[]>([]);
  isCreateVisible = false;

  constructor(private completedWorkoutsService: CompletedWorkoutsService) {
    this.completedWorkoutsService.getAll().subscribe(w => this.completedWorkouts.set(w));
  }

  reload() {
    this.completedWorkoutsService.getAll().subscribe(w => {
      this.completedWorkouts.set(w);
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