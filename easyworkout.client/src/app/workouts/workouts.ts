import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { WorkoutsService } from './workouts.service';
import { WorkoutResponse } from '../model/interfaces';
import { WorkoutComponent } from '../components/workout/workout';
import { CreateWorkout } from '../components/create-workout/create-workout';

@Component({
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, MatButtonModule, MatIconModule, WorkoutComponent, CreateWorkout],
  templateUrl: './workouts.html',
  styleUrl: './workouts.css'
})
export class WorkoutsComponent {
  workouts = signal<WorkoutResponse[]>([]);
  isLoaded = signal(false);
  isCreateVisible = false;

  constructor(private service: WorkoutsService) {
    this.reload();
  }

  reload() {
    this.service.getAll().subscribe(w => {
      this.workouts.set(w);
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