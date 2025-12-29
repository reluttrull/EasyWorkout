import { Component, input, output, inject } from '@angular/core';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { ExercisesService } from '../../exercises/exercises.service';
import { ExerciseSetResponse } from '../../model/interfaces';

@Component({
  selector: 'app-set',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './set.html',
  styleUrl: './set.css',
})
export class SetComponent {
  exercisesService = inject(ExercisesService);
  set = input.required<ExerciseSetResponse>();
  exerciseId = input.required<string>();
  onSetChanged = output();

  
  delete(setId:string) {
    if(confirm('Are you sure you want to delete this set?')) {
      console.log('id', this.set().id);
      this.exercisesService.deleteSet(this.exerciseId(), setId)
        .subscribe({
          next: () => {
            this.onSetChanged.emit();
          },
          error: err => console.error(err)
        });
      console.log("Item deleted");
    } else {
      console.log("Deletion cancelled");
    }
  }
}
