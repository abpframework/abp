import { DOCUMENT } from '@angular/common';
import { Injectable, inject, signal } from '@angular/core';
import { fromEvent, merge, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService{
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;

  networkStatus$ = merge(
    of(navigator.onLine),
    fromEvent(window, 'offline'),
    fromEvent(window, 'online')
  ).pipe(map(() => navigator.onLine));
}

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionSignalService{
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;
  readonly networkStatus = signal(navigator.onLine);
  
  constructor(){
    window.addEventListener('offline', () => { this.networkStatus.set(navigator.onLine) });
    window.addEventListener('online', () => { this.networkStatus.set(navigator.onLine) });
  }
}
