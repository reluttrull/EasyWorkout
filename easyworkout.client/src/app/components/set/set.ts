import { Component, input, computed } from '@angular/core';
import { Set } from '../../model/interfaces';
import { WeightUnit, DurationUnit } from '../../model/enums';

@Component({
  selector: 'app-set',
  imports: [],
  templateUrl: './set.html',
  styleUrl: './set.css',
})
export class SetComponent {
  set = input.required<Set>();
}
