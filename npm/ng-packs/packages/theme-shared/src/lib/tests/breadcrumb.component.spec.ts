import { LocalizationPipe } from '@abp/ng.core';
import { Directive } from '@angular/core';
import { Router } from '@angular/router';
import { createComponentFactory, Spectator, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { BreadcrumbComponent } from '../components/breadcrumb/breadcrumb.component';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[routerLink], [routerLinkActive]',
})
class DummyRouterLinkDirective {}

describe('BreadcrumbComponent', () => {
  let spectator: Spectator<BreadcrumbComponent>;
  let store: SpyObject<Store>;
  const createComponent = createComponentFactory({
    component: BreadcrumbComponent,
    mocks: [Store, Router],
    imports: [],
    declarations: [LocalizationPipe, DummyRouterLinkDirective],
    detectChanges: false,
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.get(Store);
  });

  it('should display the breadcrumb', () => {
    const router = spectator.get(Router);
    (router as any).url = '/identity/users';
    store.selectSnapshot.mockReturnValueOnce({ LeptonLayoutState: {} });
    store.selectSnapshot.mockReturnValueOnce({
      name: 'Identity',
      children: [{ name: 'Users', path: 'users' }],
    });
    // for abpLocalization
    store.selectSnapshot.mockReturnValueOnce('Identity');
    store.selectSnapshot.mockReturnValueOnce('Users');
    spectator.detectChanges();

    expect(spectator.component.segments).toEqual(['Identity', 'Users']);
    const elements = spectator.queryAll('li');
    expect(elements).toHaveLength(3);
    expect(elements[1]).toHaveText('Identity');
    expect(elements[2]).toHaveText('Users');
  });
});
