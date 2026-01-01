import { Component, OnInit, signal, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { WorkoutsService } from '../workouts/workouts.service';
import { CompletedWorkoutsService } from '../completed-workouts/completed-workouts.service';
import {
  WorkoutResponse,
  FinishWorkoutRequest,
  FinishExerciseRequest,
  FinishExerciseSetRequest
} from '../model/interfaces';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  selector: 'app-do-workout',
  providers: [OrderByPipe],
  imports: [MatButtonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule],
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
    lastEditedDate: new Date(0),
    name: '',
    notes: '',
    exercises: []
  });
  orderedExercises: WorkoutResponse['exercises'] = [];

  form = this.fb.group({
    exercises: this.fb.array<FormGroup>([])
  });

  
  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private workoutsService: WorkoutsService, 
    private completedWorkoutsService : CompletedWorkoutsService,
    private orderByPipe: OrderByPipe) {}

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

    for (const exercise of this.workoutGoal().exercises) {
      const setsArray = this.fb.array<FormGroup>([]);

      exercise.exerciseSets = this.orderByPipe.transform(
        exercise.exerciseSets,
        'setNumber',
        'asc'
      );

      for (const set of exercise.exerciseSets) {
        setsArray.push(
          this.fb.group({
            exerciseSetId: [set.id],
            completedDate: [new Date().toJSON()],
            setNumber: [set.setNumber],
            weight: [null],
            reps: [null],
            duration: [null],
            distance: [null]
          })
        );
      }

      this.exercisesArray.push(
        this.fb.group({
          exerciseId: [exercise.id],
          completedDate: [new Date().toJSON()],
          exerciseNumber: [exercise.exerciseNumber],
          sets: setsArray
        })
      );
    }

    this.orderedExercises = this.workoutGoal().exercises;
  }

  submit() {

    const request: FinishWorkoutRequest = {
      workoutId: this.id!,
      completedDate: new Date(),
      completedNotes: null,
      completedExercises: []
    };

    for (const exercise of this.form.value.exercises ?? []) {

      const completedExercise: FinishExerciseRequest = {
        exerciseId: exercise.exerciseId!,
        completedDate: exercise.completedDate!,
        exerciseNumber: exercise.exerciseNumber!,
        completedExerciseSets: []
      };

      for (const set of exercise.sets ?? []) {
        completedExercise.completedExerciseSets.push({
          exerciseSetId: set.exerciseSetId!,
          completedDate: set.completedDate!,
          setNumber: set.setNumber!,
          reps: set.reps,
          weight: set.weight,
          duration: set.duration,
          distance: set.distance
        });
      }

      request.completedExercises.push(completedExercise);
    }

    this.completedWorkoutsService.create(request).subscribe({
      next: () => this.router.navigate(['completed-workouts']),
      error: err => console.error(err)
    });
  }
}
