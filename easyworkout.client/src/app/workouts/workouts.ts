import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkoutsService } from './workouts.service';
import { Workout } from './workouts.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './workouts.html'
})
export class WorkoutsComponent {
  workouts = signal<Workout[]>([]);

  constructor(private service: WorkoutsService) {
    this.service.getAll().subscribe(w => this.workouts.set(w));
  }
}