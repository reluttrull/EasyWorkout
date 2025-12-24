import { Component, OnInit, signal, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { WorkoutsService } from '../workouts/workouts.service';
import { WorkoutResponse } from '../model/interfaces';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  selector: 'app-do-workout',
  imports: [ReactiveFormsModule, OrderByPipe],
  templateUrl: './do-workout.html',
  styleUrl: './do-workout.css',
})
export class DoWorkout implements OnInit {
  fb = inject(FormBuilder);
  id: string | null = null;
  workoutGoal = signal<WorkoutResponse>({
    id: '',
    addedByUserId: '',
    addedDate: new Date(0),
    name: '',
    notes: '',
    exercises: []
  });
  form = this.fb.group({
    sets: this.fb.array<FormGroup>([])
  });
  
  constructor(private route: ActivatedRoute, private workoutsService: WorkoutsService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
    });
    this.workoutsService.get(this.id??'').subscribe(w => {
      this.workoutGoal.set(w);
      this.initForm();
    });
  }

  get setsArray() {
    return this.form.controls.sets;
  }

  initForm() {
    this.form.controls.sets.clear();

    for (const exercise of this.workoutGoal().exercises) {
      for (const set of exercise.exerciseSets) {
        this.form.controls.sets.push(
          this.fb.group({
            setId: [set.id],
            weight: [set.weight ?? null],
            reps: [set.reps ?? null],
            duration: [set.duration ?? null]
          })
        );
      }
    }
  }

  submit() {
    const completedSets = this.form.value.sets;
    console.log(completedSets);
  }
}
