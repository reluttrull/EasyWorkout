import { Component, inject, signal } from '@angular/core';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { GreetingPipe } from '../../pipes/greeting-pipe';
import { AccountService } from '../../account/account.service';
import { CompletedWorkoutsService } from '../../completed-workouts/completed-workouts.service';

@Component({
  selector: 'app-home',
  imports: [MatIconModule, MatButtonModule, DatePipe, GreetingPipe],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  accountService = inject(AccountService);
  user = this.accountService.user;
  lastCompletedDate = signal<Date | null>(null);

  constructor(private completedWorkoutsService: CompletedWorkoutsService) {
    this.completedWorkoutsService.getLastCompletedDate().subscribe(d => {
      this.lastCompletedDate.set(d);
    });
  }
}
