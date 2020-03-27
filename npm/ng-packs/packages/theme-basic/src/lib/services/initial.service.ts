import { LazyLoadService, AddReplaceableComponent } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import styles from '../constants/styles';
import { ApplicationLayoutComponent } from '../components/application-layout/application-layout.component';
import { AccountLayoutComponent } from '../components/account-layout/account-layout.component';
import { EmptyLayoutComponent } from '../components/empty-layout/empty-layout.component';

@Injectable({ providedIn: 'root' })
export class InitialService {
  constructor(private lazyLoadService: LazyLoadService, private store: Store) {
    this.appendStyle().subscribe();
    this.store.dispatch([
      new AddReplaceableComponent({
        key: 'Theme.ApplicationLayoutComponent',
        component: ApplicationLayoutComponent,
      }),
      new AddReplaceableComponent({
        key: 'Theme.AccountLayoutComponent',
        component: AccountLayoutComponent,
      }),
      new AddReplaceableComponent({
        key: 'Theme.EmptyLayoutComponent',
        component: EmptyLayoutComponent,
      }),
    ]);
  }

  appendStyle() {
    return this.lazyLoadService.load(null, 'style', styles, 'head', 'beforeend');
  }
}
