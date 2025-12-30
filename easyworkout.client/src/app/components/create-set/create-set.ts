import { Component, input, output, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import { ExercisesService } from '../../exercises/exercises.service';
import { WeightUnit, DurationUnit } from '../../model/enums';
import { CreateSetRequest } from '../../model/interfaces';

@Component({
  selector: 'app-create-set',
  imports: [ReactiveFormsModule, MatIconModule, MatButtonModule, 
    MatFormFieldModule, MatInputModule, MatSelectModule],
  templateUrl: './create-set.html',
  styleUrl: './create-set.css',
})
export class CreateSet {  
  fb = inject(FormBuilder);
  exercisesService = inject(ExercisesService);
  exerciseId = input.required<string>();
  onCreate = output();
  onReturn = output();
  form:FormGroup = this.fb.nonNullable.group({
    reps: [null],
    weight: [null],
    weightUnit: [null],
    duration: [null],
    durationUnit: [null]
  });
  validationErrors = signal<string[]>([]);

  public readonly WeightUnit = WeightUnit;
  public readonly DurationUnit = DurationUnit;
  public weightUnitOptions = Object.values(this.WeightUnit);
  public durationUnitOptions = Object.values(this.DurationUnit);
  
  submit() {
    this.validationErrors.set([]);
    
    const setRequest: CreateSetRequest = {
      reps: this.form.value.reps,
      weight: this.form.value.weight,
      weightUnit: this.form.value.weightUnit,
      duration: this.form.value.duration,
      durationUnit: this.form.value.durationUnit
    };

    this.exercisesService.createSet(this.exerciseId(), setRequest).subscribe({
      next: (result) => {
        this.onCreate.emit();
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
