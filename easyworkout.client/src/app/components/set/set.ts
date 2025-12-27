import { Component, input, output, inject } from '@angular/core';
import { ExercisesService } from '../../exercises/exercises.service';
import { Set } from '../../model/interfaces';

@Component({
  selector: 'app-set',
  imports: [],
  templateUrl: './set.html',
  styleUrl: './set.css',
})
export class SetComponent {
  exercisesService = inject(ExercisesService);
  set = input.required<Set>();
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
