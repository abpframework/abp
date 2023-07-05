import { Injectable } from '@angular/core';
import { fromEvent, merge, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService{
  networkStatus$ = merge(
    of(null),
    fromEvent(window, 'offline'),
    fromEvent(window, 'online')
  ).pipe(map(() => navigator.onLine))
}
