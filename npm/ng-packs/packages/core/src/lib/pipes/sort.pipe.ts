import { Pipe, PipeTransform, Injectable } from '@angular/core';
import clone from 'just-clone';

export type SortOrder = 'asc' | 'desc';

@Injectable()
@Pipe({
  name: 'abpSort',
})
export class SortPipe implements PipeTransform {
  intialValue: any[];

  transform(value: any[], sortOrder: SortOrder | string = 'asc', sortKey?: string): any {
    sortOrder = sortOrder && (sortOrder.toLowerCase() as any);

    if (!this.intialValue) this.intialValue = clone(value);

    if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc')) return this.intialValue;

    let sorted;
    if (!sortKey) {
      sorted = value.sort();
    } else {
      sorted = value.sort((a, b) => (a[sortKey] < b[sortKey] ? -1 : a[sortKey] > b[sortKey] ? 1 : 0));
    }

    return sortOrder === 'asc' ? sorted : sorted.reverse();
  }
}
