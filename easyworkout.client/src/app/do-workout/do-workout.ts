import { Component, OnInit, signal, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { WorkoutsService } from '../workouts/workouts.service';
import { WorkoutResponse } from '../model/interfaces';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  selector: 'app-do-workout',
  providers: [OrderByPipe],
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
  orderedExercises: WorkoutResponse['exercises'] = [];

  form = this.fb.group({
    exercises: this.fb.array<FormGroup>([])
  });

  
  constructor(private route: ActivatedRoute, private workoutsService: WorkoutsService, private orderByPipe: OrderByPipe) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
    });
    this.workoutsService.get(this.id??'').subscribe(w => {
      this.workoutGoal.set(w);
      console.log(this.workoutGoal());
      this.initForm();
    });
  }

  get exercisesArray() {
    return this.form.controls.exercises;
  }
initForm() {
  this.exercisesArray.clear();

  const orderedExercises = this.orderByPipe.transform(
    this.workoutGoal().exercises,
    'addedDate',
    'asc'
  );

  for (const exercise of orderedExercises) {
    const setsArray = this.fb.array<FormGroup>([]);

    exercise.exerciseSets = this.orderByPipe.transform(
      exercise.exerciseSets,
      'setNumber',
      'asc'
    );

    for (const set of exercise.exerciseSets) {
      setsArray.push(
        this.fb.group({
          setId: [set.id],
          weight: [null],
          reps: [null],
          duration: [null]
        })
      );
    }

    this.exercisesArray.push(
      this.fb.group({
        exerciseId: [exercise.id],
        sets: setsArray
      })
    );
  }

  this.orderedExercises = orderedExercises;
}


  submit() {
    console.log(this.form.value.exercises);
  }

}
