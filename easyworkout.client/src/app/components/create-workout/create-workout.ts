import { Component, output, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { WorkoutsService } from '../../workouts/workouts.service';
import { CreateWorkoutRequest } from '../../model/interfaces';

@Component({
  selector: 'app-create-workout',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './create-workout.html',
  styleUrl: './create-workout.css',
})
export class CreateWorkout {
  fb = inject(FormBuilder);
  router = inject(Router);
  workoutsService = inject(WorkoutsService);
  onReturn = output();
  form:FormGroup = this.fb.nonNullable.group({
    name: [''],
    notes: ['']
  });
  validationErrors = signal<string[]>([]);

  submit() {
    this.validationErrors.set([]);

    const workoutRequest: CreateWorkoutRequest = {
      name: this.form.value.name,
      notes: this.form.value.notes
    };

    this.workoutsService.create(workoutRequest).subscribe({
      next: () => {
        this.onReturn.emit();
      },
      error: (err:any) => {
        if (err.status == 400 && err.error?.errors) {
          for (const key of Object.keys(err.error.errors)) {
            this.validationErrors.update(errs => [...errs, ...err.error.errors[key]]);
          }
        } else {
          this.validationErrors.update(errs => [...errs, 'An unexpected error occurred.']);
        };
        return;
      }
    });
  }


  return() {
    this.form.reset();
    this.onReturn.emit();
  }
}
