import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkoutsService } from './workouts.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './workouts.html'
})
export class WorkoutsComponent {
  workouts: any[] = [];

  constructor(private service: WorkoutsService) {
    this.service.getAll().subscribe(w => this.workouts = w);
  }
}