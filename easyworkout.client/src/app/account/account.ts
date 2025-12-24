import { Component, signal } from '@angular/core';
import { AccountService } from './account.service';
import { UserResponse } from '../model/interfaces';

@Component({
  selector: 'app-account',
  imports: [],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account {
  user = signal<UserResponse>({
    firstName: '',
    lastName: '',
    joinedDate: new Date(0)
  });

  constructor(private accountService: AccountService) { 
    this.accountService.get().subscribe(u => this.user.set(u));
  }
}
