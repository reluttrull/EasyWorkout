import { Component, output, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import { ExercisesService } from '../../exercises/exercises.service';
import { CreateExerciseRequest, ExerciseResponse } from '../../model/interfaces';

@Component({
  selector: 'app-create-exercise',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './create-exercise.html',
  styleUrl: './create-exercise.css',
})
export class CreateExercise {
  onCreate = output<string>();
  onReturn = output();
  form!: FormGroup;
  validationErrors = signal<string[]>([]);
  
  constructor(private fb: FormBuilder, private router: Router, private exercisesService: ExercisesService) {
    this.form = this.fb.nonNullable.group({
      name: [''],
      notes: ['']
    });
  }

  submit() {
    this.validationErrors.set([]);

    const exerciseRequest: CreateExerciseRequest = {
      name: this.form.value.name,
      notes: this.form.value.notes
    };

    this.exercisesService.create(exerciseRequest).subscribe({
      next: (res) => {
        this.onCreate.emit(res.id);
        this.onReturn.emit();
      },
      error: (err) => {
        if (err.status == 400 && err.error.errors) {
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
