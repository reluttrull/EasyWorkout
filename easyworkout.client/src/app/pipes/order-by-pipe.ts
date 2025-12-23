import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderBy',
  standalone: true
})
export class OrderByPipe implements PipeTransform {

  transform(value: any[], property?: string, direction: 'asc' | 'desc' = 'asc'): any[] {
    console.log('using pipe');
    if (!value || !Array.isArray(value) || value.length <= 1) {
      return value;
    }

    return [...value].sort((a, b) => {
      const aValue = property ? a[property] : a;
      const bValue = property ? b[property] : b;

      let comparison = 0;
      if (typeof aValue == 'string' && typeof bValue == 'string') {
        comparison = aValue.localeCompare(bValue);
      } else if (aValue > bValue) {
        comparison = 1;
      } else if (aValue < bValue) {
        comparison = -1;
      }

      return direction == 'asc' ? comparison : comparison * -1;
    });
  }

}
