import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WorkoutsService } from '../workouts/workouts.service';
import { CompletedWorkoutsService } from '../completed-workouts/completed-workouts.service';
import { FinishWorkoutRequest, FinishExerciseRequest } from '../model/interfaces';
import { WeightUnit, DurationUnit, DistanceUnit } from '../model/enums';

@Component({
  selector: 'app-do-workout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSelectModule, MatProgressSpinnerModule,
    MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule],
  templateUrl: './do-workout.html',
})
export class DoWorkout implements OnInit {
  id: string | null = null;
  isLoading = signal(true);
  workoutName = signal('');
  route = inject(ActivatedRoute);
  router = inject(Router);
  fb = inject(FormBuilder);
  form = this.fb.group({
    exercises: this.fb.array<FormGroup>([]),
    newExerciseName: ['']
  });
  public readonly WeightUnit = WeightUnit;
  public readonly DurationUnit = DurationUnit;
  public readonly DistanceUnit = DistanceUnit;
  public weightUnitOptions = Object.values(this.WeightUnit);
  public durationUnitOptions = Object.values(this.DurationUnit);
  public distanceUnitOptions = Object.values(this.DistanceUnit);

  constructor(
    private workoutsService: WorkoutsService,
    private completedWorkoutsService: CompletedWorkoutsService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id = params.get('id');
      
      this.workoutsService.get(this.id ?? '').subscribe(w => {
        console.log('got a response', w);
        this.workoutName.set(w.name);
        
        this.exercises.clear();
        
        for (let exIndex: number = 0; exIndex < w.exercises.length; exIndex++) {
          let exercise = w.exercises[exIndex];
          let exerciseFormGroup = this.fb.group({
            name: [exercise.name],
            exerciseId: [exercise.id],
            completedDate: [new Date().toJSON()],
            sets: this.fb.array<FormGroup>([])
          });
          this.exercises.push(exerciseFormGroup);

          for (let setIndex: number = 0; setIndex < exercise.exerciseSets.length; setIndex++) {
            let set = exercise.exerciseSets[setIndex];
            let setFormGroup = this.fb.group({
              exerciseSetId: [set.id],
              completedDate: [new Date().toJSON()],
              reps: [null],
              goalReps: [set.reps],
              weight: [null],
              goalWeight: [set.weight],
              weightUnit: [set.weightUnit],
              duration: [null],
              goalDuration: [set.duration],
              durationUnit: [set.durationUnit],
              distance: [null],
              goalDistance: [set.distance],
              distanceUnit: [set.distanceUnit]
            });
            this.sets(exIndex).push(setFormGroup);
          }
        }
        this.isLoading.set(false);
      });
    });
  }

  /* ------------------ Getters ------------------ */

  get exercises(): FormArray {
    return this.form.get('exercises') as FormArray;
  }

  sets(exIndex: number): FormArray {
    return this.exercises.at(exIndex).get('sets') as FormArray;
  }

  /* ------------------ Builders ------------------ */

  createEmptyExercise(): FormGroup {
    let name = this.form.get('newExerciseName');
    let nameVal = name?.value;
    name?.reset();
    
    return this.fb.group({
      name: [nameVal],
      exerciseId: [null],
      completedDate: [new Date().toJSON()],
      sets: this.fb.array([])
    });
  }

  createEmptySet(exIndex: number): FormGroup {
    return this.fb.group({
      exerciseSetId: [null],
      completedDate: [new Date().toJSON()],
      reps: [null],
      goalReps: [null],
      weight: [null],
      goalWeight: [null],
      weightUnit: [null],
      duration: [null],
      goalDuration: [null],
      durationUnit: [null],
      distance: [null],
      goalDistance: [null],
      distanceUnit: [null]
    });
  }

  /* ------------------ Actions ------------------ */

  addEmptyExercise() {
    this.exercises.push(this.createEmptyExercise());
  }

  removeExercise(index: number) {
    this.exercises.removeAt(index);
  }

  addEmptySet(exIndex: number) {
    this.sets(exIndex).push(this.createEmptySet(exIndex));
  }

  removeSet(exIndex: number, setIndex: number) {
    this.sets(exIndex).removeAt(setIndex);
  }

  submit() {
    if (this.form.valid) {
      const exercises: FinishExerciseRequest[] = [];
      
      this.exercises.controls.forEach((exerciseControl, exIndex) => {
        const exerciseValue = exerciseControl.value;
        const sets = this.sets(exIndex);
        
        const completedExercise: FinishExerciseRequest = {
          exerciseId: exerciseValue.exerciseId,
          fallbackName: exerciseValue.name,
          completedDate: exerciseValue.completedDate,
          exerciseNumber: exIndex, // use index as exerciseNumber
          completedExerciseSets: []
        };

        sets.controls.forEach((setControl, setIndex) => {
          const setVal = setControl.value;
          completedExercise.completedExerciseSets.push({
            exerciseSetId: setVal.exerciseSetId,
            completedDate: setVal.completedDate,
            setNumber: setIndex, // use index as setNumber
            reps: setVal.reps,
            goalReps: setVal.goalReps,
            weight: setVal.weight,
            goalWeight: setVal.goalWeight,
            weightUnit: setVal.weightUnit,
            duration: setVal.duration,
            goalDuration: setVal.goalDuration,
            durationUnit: setVal.durationUnit,
            distance: setVal.distance,
            goalDistance: setVal.goalDistance,
            distanceUnit: setVal.distanceUnit
          });
        });

        exercises.push(completedExercise);
      });

      console.log('Submitting exercises:', exercises);

      const request: FinishWorkoutRequest = {
        workoutId: this.id ?? '',
        completedDate: new Date(),
        completedExercises: exercises
      };
      this.completedWorkoutsService.create(request).subscribe({
        next: () => this.router.navigate(['completed-workouts']),
        error: err => console.error(err)
      });
    }
  }
}
