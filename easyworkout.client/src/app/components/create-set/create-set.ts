import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ExercisesService } from '../../exercises/exercises.service';
import { WeightUnit, DurationUnit } from '../../model/enums';
import { CreateSetRequest } from '../../model/interfaces';

@Component({
  selector: 'app-create-set',
  imports: [ReactiveFormsModule],
  templateUrl: './create-set.html',
  styleUrl: './create-set.css',
})
export class CreateSet {  
  exerciseId = input.required<string>();
  onCreate = output();
  onReturn = output();
  form!: FormGroup;

  public readonly WeightUnit = WeightUnit;
  public readonly DurationUnit = DurationUnit;
  public weightUnitOptions = Object.values(this.WeightUnit);
  public durationUnitOptions = Object.values(this.DurationUnit);

  constructor(private fb: FormBuilder, private exercisesService: ExercisesService) {
    this.form = this.fb.nonNullable.group({
      reps: [null],
      weight: [null],
      weightUnit: [null],
      duration: [null],
      durationUnit: [null],
      notes: [null]
    });
  }
  
  submit() {
    const setRequest: CreateSetRequest = {
      reps: this.form.value.reps,
      weight: this.form.value.weight,
      weightUnit: this.form.value.weightUnit,
      duration: this.form.value.duration,
      durationUnit: this.form.value.durationUnit,
      notes: this.form.value.notes
    };

    this.exercisesService.createSet(this.exerciseId(), setRequest).subscribe({
      next: (result) => {
        this.onCreate.emit();
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
