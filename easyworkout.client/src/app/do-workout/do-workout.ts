import { Component, OnInit, signal, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { WorkoutsService } from '../workouts/workouts.service';
import { CompletedWorkoutsService } from '../completed-workouts/completed-workouts.service';
import { WorkoutResponse, FinishWorkoutRequest, FinishExerciseSetRequest } from '../model/interfaces';
import { OrderByPipe } from '../pipes/order-by-pipe';

@Component({
  selector: 'app-do-workout',
  providers: [OrderByPipe],
  imports: [ReactiveFormsModule],
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
            exerciseSetId: [set.id],
            completedDate: [new Date().toJSON()],
            setNumber: [set.setNumber],
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

    let request: FinishWorkoutRequest = {
      workoutId: this.id!,
      completedDate: new Date(),
      completedNotes: null,
      completedExerciseSets: []
    };

    for (const exercise of this.form.value.exercises??[]) {
      exercise.sets.forEach((set:FinishExerciseSetRequest) => {
        request.completedExerciseSets.push({
          exerciseSetId: set.exerciseSetId,
          completedDate: set.completedDate,
          setNumber: set.setNumber,
          reps: set.reps,
          weight: set.weight,
          duration: set.duration
        })
      });
    }
    
    this.completedWorkoutsService.create(request).subscribe({
      next: (result) => {
        this.router.navigate(['/workouts']);
      },
      error: err => console.error(err)
    });
  }

}
