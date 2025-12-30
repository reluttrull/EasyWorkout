import { Component, input, output, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { WorkoutResponse } from '../../model/interfaces';
import { ExerciseComponent } from '../exercise/exercise';
import { AddExercise } from '../add-exercise/add-exercise';
import { WorkoutsService } from '../../workouts/workouts.service';
import { OrderByPipe } from '../../pipes/order-by-pipe';

@Component({
  selector: 'app-workout',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, 
    MatFormFieldModule, MatInputModule, MatMenuModule, ExerciseComponent, AddExercise, DatePipe, OrderByPipe],
  templateUrl: './workout.html',
  styleUrl: './workout.css',
})
export class WorkoutComponent {
  fb = inject(FormBuilder);
  router = inject(Router);
  workoutsService = inject(WorkoutsService);
  form:FormGroup = this.fb.nonNullable.group({
    name: [''],
    notes: ['']
  });
  validationErrors = signal<string[]>([]);
  onReturn = output();
  workout = input.required<WorkoutResponse>();
  onWorkoutChanged = output();
  workoutDetail = false;
  isEditMode = false;
  isAddMode = false;

  toggleWorkoutDetail() {
    this.workoutDetail = !this.workoutDetail;
  }

  openAddExercise() {
    this.isAddMode = true;
  }

  closeAddExercise() {
    this.isAddMode = false;
    this.reloadWorkout();
  }

  reloadWorkout() {
    this.onWorkoutChanged.emit();
  }

  startWorkout() {
    this.router.navigate([`/do-workout/${this.workout().id}`]);
  }

  edit() {
    this.form.setValue({
      name: this.workout().name,
      notes: this.workout().notes
    });
    this.isEditMode = true;
  }

  cancelEdit() {
    this.isEditMode = false;
  }

  update() {
    this.validationErrors.set([]);

    this.workoutsService.update(this.workout().id, this.form.value)
      .subscribe({
        next: () => {
          this.onWorkoutChanged.emit();
          this.isEditMode = false;
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

  delete() {
    if(confirm(`Are you sure you want to delete workout ${this.workout().name}?  It will also be deleted from every workout log it's attached to.`)) {
      console.log('id', this.workout().id);
      this.workoutsService.delete(this.workout().id)
        .subscribe({
          next: () => {
            this.onWorkoutChanged.emit();
          },
          error: err => console.error(err)
        });
      console.log("Item deleted");
    } else {
      console.log("Deletion cancelled");
    }
  }
}
