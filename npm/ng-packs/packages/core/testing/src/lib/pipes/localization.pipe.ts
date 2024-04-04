import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Injectable()
@Pipe({
  name: 'abpLocalization',
  standalone: true,
})
export class LocalizationPipe implements PipeTransform {
  transform(value: string): string {
    return value;
  }
}
