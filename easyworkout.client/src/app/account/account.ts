import { Component, inject } from '@angular/core';
import { AccountService } from './account.service';
import { UserResponse } from '../model/interfaces';

@Component({
  selector: 'app-account',
  imports: [],
  templateUrl: './account.html',
  styleUrl: './account.css',
})
export class Account {
  private accountService = inject(AccountService);
  user = this.accountService.user;
}
