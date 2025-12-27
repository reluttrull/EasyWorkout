import { Component, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { OrderByPipe } from '../../pipes/order-by-pipe';
import { CompletedWorkoutResponse } from '../../model/interfaces';

@Component({
  selector: 'app-completed-workout',
  imports: [DatePipe, OrderByPipe],
  templateUrl: './completed-workout.html',
  styleUrl: './completed-workout.css',
})
export class CompletedWorkoutComponent {
  completedWorkout = input.required<CompletedWorkoutResponse>();

}
