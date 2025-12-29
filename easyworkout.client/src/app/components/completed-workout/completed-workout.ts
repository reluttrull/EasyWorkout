import { Component, input, output, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { OrderByPipe } from '../../pipes/order-by-pipe';
import { CompletedWorkoutsService } from '../../completed-workouts/completed-workouts.service';
import { CompletedWorkoutResponse } from '../../model/interfaces';

@Component({
  selector: 'app-completed-workout',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule,
    MatFormFieldModule, MatInputModule, DatePipe, OrderByPipe],
  providers: [DatePipe],
  templateUrl: './completed-workout.html',
  styleUrl: './completed-workout.css',
})
export class CompletedWorkoutComponent {
  private fb = inject(FormBuilder);
  private completedWorkoutsService = inject(CompletedWorkoutsService);
  private datePipe = inject(DatePipe);
  form:FormGroup = this.fb.nonNullable.group({
    completedNotes: ['']
  });
  isEditMode = false;
  completedWorkout = input.required<CompletedWorkoutResponse>();
  onCompletedWorkoutChanged = output();

  
  edit() {
    this.form.setValue({
      completedNotes: this.completedWorkout().completedNotes
    });
    this.isEditMode = true;
  }
  
  cancelEdit() {
    this.isEditMode = false;
  }
  
  update() {
    this.completedWorkoutsService.update(this.completedWorkout().id, this.form.value)
      .subscribe({
        next: () => {
          this.onCompletedWorkoutChanged.emit();
          this.isEditMode = false;
        },
        error: err => console.error(err)
      });
  }

  delete() {
    if(confirm(`Are you sure you want to delete workout ${this.completedWorkout().originalName} completed on ${this.datePipe.transform(this.completedWorkout().completedDate, "MMM dd, yyyy 'at' hh:mm a")}?`)) {
      console.log('id', this.completedWorkout().id);
      this.completedWorkoutsService.delete(this.completedWorkout().id)
        .subscribe({
          next: () => {
            this.onCompletedWorkoutChanged.emit();
          },
          error: err => console.error(err)
        });
      console.log("Item deleted");
    } else {
      console.log("Deletion cancelled");
    }
  }
}
