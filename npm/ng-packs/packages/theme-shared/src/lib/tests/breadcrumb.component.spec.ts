import { LocalizationPipe } from '@abp/ng.core';
import { RouterTestingModule } from '@angular/router/testing';
import { createComponentFactory, Spectator, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { BreadcrumbComponent } from '../components/breadcrumb/breadcrumb.component';

describe('BreadcrumbComponent', () => {
  let spectator: Spectator<BreadcrumbComponent>;
  let store: SpyObject<Store>;
  const createComponent = createComponentFactory({
    component: BreadcrumbComponent,
    mocks: [Store],
    imports: [RouterTestingModule],
    declarations: [LocalizationPipe],
    detectChanges: false,
  });

  beforeEach(() => {
    spectator = createComponent();
    store = spectator.get(Store);
  });

  it('should display the breadcrumb', () => {
    store.selectSnapshot.andCallFake(selector => selector({ LeptonLayoutState: {} }));
  });
});
