import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkoutsService } from './workouts.service';
import { Workout } from '../model/interfaces';
import { WorkoutComponent } from '../components/workout/workout';

@Component({
  standalone: true,
  imports: [CommonModule, WorkoutComponent],
  templateUrl: './workouts.html',
  styleUrl: './workouts.css'
})
export class WorkoutsComponent {
  workouts = signal<Workout[]>([]);

  constructor(private service: WorkoutsService) {
    this.service.getAll().subscribe(w => this.workouts.set(w));
  }
}