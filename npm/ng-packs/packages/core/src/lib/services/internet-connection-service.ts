import { DOCUMENT } from '@angular/common';
import { Injectable, computed, inject, signal } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService {
  readonly document = inject(DOCUMENT);
  readonly window = this.document.defaultView;
  readonly navigator = this.window.navigator;

  private status$ = new BehaviorSubject<boolean>(this.navigator.onLine);

  private status = signal(this.navigator.onLine);

  networkStatus = computed(() => this.status());

  constructor() {
    this.window.addEventListener('offline', () => this.setStatus(false));
    this.window.addEventListener('online', () => this.setStatus(true));
  }

  setStatus(val: boolean) {
    this.status.set(val);
    this.status$.next(val);
  }

  get networkStatus$() {
    return this.status$.asObservable();
  }
}
