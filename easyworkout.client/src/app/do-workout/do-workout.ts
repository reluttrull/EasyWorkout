import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { WorkoutsService } from '../workouts/workouts.service';
import { WorkoutResponse } from '../model/interfaces';

@Component({
  selector: 'app-do-workout',
  imports: [],
  templateUrl: './do-workout.html',
  styleUrl: './do-workout.css',
})
export class DoWorkout implements OnInit {
  id: string | null = null;
  workoutGoal = signal<WorkoutResponse>({
    id: '',
    addedByUserId: '',
    addedDate: new Date(0),
    name: '',
    notes: '',
    exercises: []
  });
  
  constructor(private route: ActivatedRoute, private workoutsService: WorkoutsService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
    });
    this.workoutsService.get(this.id??'').subscribe(w => this.workoutGoal.set(w));
  }
}
