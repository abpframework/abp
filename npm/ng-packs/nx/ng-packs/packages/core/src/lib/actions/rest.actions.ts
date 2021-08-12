import { HttpErrorResponse } from '@angular/common/http';

export class RestOccurError {
  static readonly type = '[Rest] Error';
  constructor(public payload: HttpErrorResponse | any) {}
}
