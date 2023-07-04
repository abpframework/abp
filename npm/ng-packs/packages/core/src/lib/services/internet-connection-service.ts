import { Injectable, OnDestroy, OnInit, VERSION } from '@angular/core';
import { InternalStore } from '../utils/internal-store-utils';
import { fromEvent, merge, of, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

export interface InternetConnectionState{
  status: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class InternetConnectionService{
  private store = new InternalStore<InternetConnectionState>({ status: true });
  networkStatus$: Subscription = Subscription.EMPTY;

  constructor() {
    this.init()
  }

  private init(): void {
    this.checkNetworkStatus();
  }

  private checkNetworkStatus() {
    this.networkStatus$ = merge(
      of(null),
      fromEvent(window, 'offline'),
      fromEvent(window, 'online')
    )
    .pipe(map(() => navigator.onLine))
    .subscribe(status => {
      this.setStatus(status)
    });
  }

  getStatus(){
    return this.store.state.status
  }

  getStatus$(){
    return this.store.sliceState(({ status }) => status);
  }

  updateStatus$() {
    return this.store.sliceUpdate(({ status }) => status);
  }

  setStatus(status: boolean){
    this.store.patch({ status })
  }

}
