import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'abpSort',
    pure: false
})
export class SortPipe implements PipeTransform {
    transform(value: any[], sortOrder: string): any {
        sortOrder = sortOrder.toLowerCase();
        if(sortOrder === "desc") return value.reverse();
        else return value;
    }
}