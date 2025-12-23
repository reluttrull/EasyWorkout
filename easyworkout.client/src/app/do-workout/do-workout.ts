import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { WorkoutsService } from '../workouts/workouts.service';

@Component({
  selector: 'app-do-workout',
  imports: [],
  templateUrl: './do-workout.html',
  styleUrl: './do-workout.css',
})
export class DoWorkout implements OnInit {
  id: string | null = null;
  
  constructor(private route: ActivatedRoute, private workoutsService: WorkoutsService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
    });
  }
}
