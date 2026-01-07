import { Component, output, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { CompletedWorkoutsService } from '../../completed-workouts/completed-workouts.service';
import { WorkoutsService } from '../../workouts/workouts.service';
import { ExercisesService } from '../../exercises/exercises.service';
import { AccountService } from '../../account/account.service';

@Component({
  selector: 'app-delete-account',
  imports: [],
  templateUrl: './delete-account.html',
  styleUrl: './delete-account.css',
})
export class DeleteAccount {
  onCancel = output();

  accountService = inject(AccountService);
  completedWorkoutsService = inject(CompletedWorkoutsService);
  workoutsService = inject(WorkoutsService);
  exercisesService = inject(ExercisesService);
  router = inject(Router);
  isWaitingCW = signal(false);
  isWaitingW = signal(false);
  isWaitingE = signal(false);
  isWaitingA = signal(false);
  isWaiting = computed(() => {
    return this.isWaitingCW() || this.isWaitingW() || this.isWaitingE() || this.isWaitingA();
  });

  cancel() {
    this.onCancel.emit();
  }

  confirmDelete() {
    this.isWaitingCW.set(true);
    this.isWaitingW.set(true);
    this.isWaitingE.set(true);
    this.isWaitingA.set(true);
    if (confirm(`Press ok to delete ALL your account info.  THIS ACTION CANNOT BE UNDONE.`)) {
      this.completedWorkoutsService.deleteAll()
        .subscribe({
          next: () => {
            console.log('completed deleted');
            this.isWaitingCW.set(false);
          },
          error: err => console.error('Problem deleting completed workouts', err)
        });
      this.workoutsService.deleteAll()
        .subscribe({
          next: () => {
            console.log('workouts deleted');
            this.isWaitingW.set(false);
          },
          error: err => console.error('Problem deleting workouts', err)
        });
      this.exercisesService.deleteAll()
        .subscribe({
          next: () => {
            console.log('exercises deleted');
            this.isWaitingE.set(false);
          },
          error: err => console.error('Problem deleting exercises', err)
        });
      this.accountService.delete()
        .subscribe({
          next: () => {
            console.log('account deleted');
            this.isWaitingA.set(false);
            this.router.navigate(['register']);
          },
          error: err => console.error('Problem deleting account', err)
        });
    } else {
      console.log("Deletion cancelled");
      this.cancel();
    }
  }
}
