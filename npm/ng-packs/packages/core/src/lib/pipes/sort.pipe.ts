import { Injectable, Pipe, PipeTransform } from '@angular/core';
export type SortOrder = 'asc' | 'desc';
@Injectable()
@Pipe({
  name: 'abpSort',
})
export class SortPipe implements PipeTransform {
  transform(value: any[], sortOrder: SortOrder | string = 'asc', sortKey?: string): any {
    sortOrder = sortOrder && (sortOrder.toLowerCase() as any);
    if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc')) return value;
    let sorted;
    if (!sortKey) {
      const numberArray = [];
      const stringArray = [];
      value.forEach(item => (typeof item === 'number' ? numberArray.push(item) : stringArray.push(item)));
      sorted = numberArray.sort().concat(stringArray.sort());
    } else {
      sorted = value.sort((a, b) => {
        return (
          Number(typeof b[sortKey] === 'number') - Number(typeof a[sortKey] === 'number') ||
          (a[sortKey] < b[sortKey] ? -1 : 1)
        );
      });
    }
    return sortOrder === 'asc' ? sorted : sorted.reverse();
  }
}
