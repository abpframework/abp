import { LocalizationPipe, ABP } from '@abp/ng.core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { BreadcrumbComponent } from '../components/breadcrumb/breadcrumb.component';
import { Router } from '@angular/router';

describe('BreadcrumbComponent', () => {
  let spectator: Spectator<BreadcrumbComponent>;
  let store: SpyObject<Store>;
  const createComponent = createComponentFactory({
    component: BreadcrumbComponent,
    mocks: [Store, Router],
    imports: [RouterTestingModule],
    declarations: [LocalizationPipe],
    detectChanges: false,
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.get(Store);
  });

  it('should display the breadcrumb', () => {
    const router = spectator.get(Router);
    (router as any).url = '/identity/users';
    store.selectSnapshot.andReturn({ name: 'Identity', children: [{ name: 'Users', path: 'users' }] } as ABP.FullRoute);
    spectator.detectChanges();
  });
});
