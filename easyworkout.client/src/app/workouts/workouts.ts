import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkoutsService } from './workouts.service';
import { Workout } from '../model/interfaces';
import { WorkoutComponent } from '../components/workout/workout';
import { CreateWorkout } from '../components/create-workout/create-workout';

@Component({
  standalone: true,
  imports: [CommonModule, WorkoutComponent, CreateWorkout],
  templateUrl: './workouts.html',
  styleUrl: './workouts.css'
})
export class WorkoutsComponent {
  workouts = signal<Workout[]>([]);
  isCreateVisible = false;

  constructor(private service: WorkoutsService) {
    this.service.getAll().subscribe(w => this.workouts.set(w));
  }

  reload() {
    this.service.getAll().subscribe(w => {
      this.workouts.set(w);
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