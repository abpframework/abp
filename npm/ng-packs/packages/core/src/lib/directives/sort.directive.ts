import { Directive, ElementRef, Input, Optional, Self } from '@angular/core';
import { Table } from 'primeng/table';

@Directive({
  selector: '[abpSort]',
})
export class SortDirective {
  constructor(private elementRef: ElementRef, @Optional() @Self() table: Table) {
    console.warn(elementRef);
    setInterval(() => console.warn(table.value), 1000);
  }
}
