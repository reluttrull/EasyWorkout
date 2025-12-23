import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { WorkoutsService } from '../../workouts/workouts.service';
import { ExercisesService } from '../../exercises/exercises.service';
import { Exercise } from '../../model/interfaces';
import { SetComponent } from '../set/set';
import { CreateSet } from '../create-set/create-set';

@Component({
  selector: 'app-exercise',
  imports: [ReactiveFormsModule, SetComponent, CreateSet],
  templateUrl: './exercise.html',
  styleUrl: './exercise.css',
})
export class ExerciseComponent {
  form!: FormGroup;
  exercise = input.required<Exercise>();
  workoutId = input<string>();
  onExerciseChanged = output();
  exerciseDetail = false;
  isEditMode = false;
  isCreateSetVisible = false;

  constructor(
    private workoutsService: WorkoutsService, 
    private exercisesService: ExercisesService,
    private fb:FormBuilder) {
      this.form = this.fb.nonNullable.group({
        name: [''],
        notes: ['']
      });
  }

  toggleExerciseDetail() {
    this.exerciseDetail = !this.exerciseDetail;
  }  
  
  clickCreateSet() {
    this.isCreateSetVisible = true;
  }  

  returnCreateSet() {
    this.isCreateSetVisible = false;
  }

  edit() {
    this.form.setValue({
      name: this.exercise().name,
      notes: this.exercise().notes
    });
    this.isEditMode = !this.isEditMode;
  }

  cancelEdit() {
    this.isEditMode = false;
  }

  reload() {
    this.onExerciseChanged.emit();
  }

  update() {
    this.exercisesService.update(this.exercise().id, this.form.value)
      .subscribe({
        next: () => {
          this.onExerciseChanged.emit();
          this.isEditMode = false;
        },
        error: err => console.error(err)
      });
  }
  
  remove(exerciseId:string) {
    this.workoutsService.removeExercise(this.workoutId()??'', exerciseId).subscribe({
      next: () => {
        this.onExerciseChanged.emit();
      },
      error: err => console.error(err)
    });
  }
  
  delete(exerciseId:string) {
    if(confirm(`Are you sure you want to delete exercise ${this.exercise().name}?  It will also be deleted from every workout it's attached to.`)) {
      console.log('id', this.exercise().id);
      this.exercisesService.delete(this.exercise().id)
        .subscribe({
          next: () => {
            this.onExerciseChanged.emit();
          },
          error: err => console.error(err)
        });
      console.log("Item deleted");
    } else {
      console.log("Deletion cancelled");
    }
  }
}
