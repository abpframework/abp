import { DOCUMENT } from '@angular/common';
import { Injectable, computed, inject, signal } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService{
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;

  /* observable */
  private status$ = new BehaviorSubject<boolean>(navigator.onLine)

  /* creates writableSignal */
  private status = signal(navigator.onLine);
  
  constructor(){
    this.window.addEventListener('offline', () => this.setStatus());
    this.window.addEventListener('online', () => this.setStatus());
  }

  private setStatus(){
    this.status.set(navigator.onLine)
    this.status$.next(navigator.onLine)
  }
  
  /* returns READONLY ANGULAR SIGNAL */
  get networkStatus(){
    return computed(this.status)
  }

  /* returns OBSERVABLE */
  get networkStatus$(){
    return this.status$
  }
}
