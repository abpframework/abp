import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'abpSort',
  // tslint:disable-next-line: no-pipe-impure
  pure: false
})
export class SortPipe implements PipeTransform {
  transform(value: any[], sortOrder: string): any {
    sortOrder = sortOrder.toLowerCase();
    if (sortOrder === 'desc') return value.reverse();
    else return value;
  }
}
