import { Injectable } from '@angular/core';
import type { OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import type { Observable, PartialObserver } from 'rxjs';

@Injectable()
export class SubscriptionService implements OnDestroy {
  private subscription = new Subscription();

  get isClosed() {
    return this.subscription.closed;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  reset() {
    this.subscription.unsubscribe();
    this.subscription = new Subscription();
  }

  subscribe<T extends unknown>(
    source$: Observable<T>,
    next?: (value: T) => void,
    error?: (error: any) => void,
  ): Subscription;
  subscribe<T extends unknown>(source$: Observable<T>, observer?: PartialObserver<T>): Subscription;
  subscribe<T extends unknown>(
    source$: Observable<T>,
    nextOrObserver?: PartialObserver<T> | Next<T>,
    error?: (error: any) => void,
  ): Subscription {
    const subscription = source$.subscribe(nextOrObserver as Next<T>, error);
    this.subscription.add(subscription);
    return subscription;
  }

  unsubscribe(subscription: Subscription | undefined | null) {
    if (!subscription) return;
    this.subscription.remove(subscription);
    subscription.unsubscribe();
  }

  unsubscribeAll() {
    this.subscription.unsubscribe();
  }
}

type Next<T> = (value: T) => void;
