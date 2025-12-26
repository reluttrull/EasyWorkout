import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { WorkoutResponse } from '../../model/interfaces';
import { ExerciseComponent } from '../exercise/exercise';
import { AddExercise } from '../add-exercise/add-exercise';
import { WorkoutsService } from '../../workouts/workouts.service';
import { OrderByPipe } from '../../pipes/order-by-pipe';

@Component({
  selector: 'app-workout',
  imports: [ReactiveFormsModule, ExerciseComponent, AddExercise, DatePipe, OrderByPipe],
  templateUrl: './workout.html',
  styleUrl: './workout.css',
})
export class WorkoutComponent {
  form!: FormGroup;
  onReturn = output();
  workout = input.required<WorkoutResponse>();
  onWorkoutChanged = output();
  workoutDetail = false;
  isEditMode = false;
  isAddMode = false;

  constructor(private workoutsService: WorkoutsService, private fb:FormBuilder, private router:Router) {
    this.form = this.fb.nonNullable.group({
      name: [''],
      notes: ['']
    });
  }

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
    this.workoutsService.update(this.workout().id, this.form.value)
      .subscribe({
        next: () => {
          this.onWorkoutChanged.emit();
          this.isEditMode = false;
        },
        error: err => console.error(err)
      });
  }

  delete() {
    if(confirm("Are you sure you want to delete workout " + this.workout().name + "?")) {
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
