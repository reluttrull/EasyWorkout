import { Pipe, PipeTransform } from '@angular/core';
import { UserResponse } from '../model/interfaces';

@Pipe({
  name: 'greeting',
})
export class GreetingPipe implements PipeTransform {

  transform(value: UserResponse | null): string {
    return `Welcome, ${value?.firstName}!`;
  }

}
