import { Component, output, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { WorkoutsService } from '../../workouts/workouts.service';
import { CreateWorkoutRequest } from '../../model/interfaces';

@Component({
  selector: 'app-create-workout',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule],
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

  submit() {
    const workoutRequest: CreateWorkoutRequest = {
      name: this.form.value.name,
      notes: this.form.value.notes
    };

    this.workoutsService.create(workoutRequest).subscribe({
      next: () => {
        this.onReturn.emit();
      },
      error: err => console.error(err)
    });
  }


  return() {
    this.form.reset();
    this.onReturn.emit();
  }
}
