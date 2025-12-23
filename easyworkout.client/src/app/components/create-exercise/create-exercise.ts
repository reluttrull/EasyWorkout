import { Component, output } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ExercisesService } from '../../exercises/exercises.service';
import { CreateExerciseRequest } from '../../model/interfaces';

@Component({
  selector: 'app-create-exercise',
  imports: [ReactiveFormsModule],
  templateUrl: './create-exercise.html',
  styleUrl: './create-exercise.css',
})
export class CreateExercise {
  onCreate = output<string>();
  onReturn = output();
  form!: FormGroup;
  
  constructor(private fb: FormBuilder, private router: Router, private exercisesService: ExercisesService) {
    this.form = this.fb.nonNullable.group({
      name: [''],
      notes: ['']
    });
  }

  submit() {
    const exerciseRequest: CreateExerciseRequest = {
      name: this.form.value.name,
      notes: this.form.value.notes
    };

    this.exercisesService.create(exerciseRequest).subscribe({
      next: (result) => {
        this.onCreate.emit(result.id);
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
