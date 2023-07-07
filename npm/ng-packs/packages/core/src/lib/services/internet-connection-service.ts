import { DOCUMENT } from '@angular/common';
import { Injectable, computed, inject, signal } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService{
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly navigator = this.window.navigator;

  private status$ = new BehaviorSubject<boolean>(navigator.onLine)

  private status = signal(navigator.onLine);

  networkStatus = computed(() => this.status())
  
  constructor(){
    this.window.addEventListener('offline', () => this.setStatus());
    this.window.addEventListener('online', () => this.setStatus());
  }

  private setStatus(){
    this.status.set(navigator.onLine)
    this.status$.next(navigator.onLine)
  }

  get networkStatus$(){
    return this.status$.asObservable()
  }
}
