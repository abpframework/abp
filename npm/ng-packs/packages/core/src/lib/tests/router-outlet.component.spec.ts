import { Spectator, createComponentFactory } from '@ngneat/spectator/jest';
import { provideRouter } from '@angular/router';
import { RouterOutletComponent } from '../components/router-outlet.component';

describe('RouterOutletComponent', () => {
  let spectator: Spectator<RouterOutletComponent>;
  const createComponent = createComponentFactory({
    component: RouterOutletComponent,
    providers: [provideRouter([{ path: '' }])],
  });

  it('should have a router-outlet element', () => {
    spectator = createComponent();
    expect((spectator.debugElement.nativeElement as HTMLElement).children.length).toBe(1);
    expect((spectator.debugElement.nativeElement as HTMLElement).children[0].tagName).toBe(
      'ROUTER-OUTLET',
    );
  });
});
