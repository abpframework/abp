import { Spectator, createComponentFactory, createHostFactory } from '@ngneat/spectator/jest';
import { RouterTestingModule } from '@angular/router/testing';
import { RouterOutletComponent } from '../components/router-outlet.component';

describe('RouterOutletComponent', () => {
  let spectator: Spectator<RouterOutletComponent>;
  const createHost = createHostFactory({
    component: RouterOutletComponent,
    imports: [RouterTestingModule],
  });

  it('should have a router-outlet element', () => {
    spectator = createHost('<abp-router-outlet></abp-router-outlet>');
    expect((spectator.debugElement.nativeElement as HTMLElement).children.length).toBe(1);
    expect((spectator.debugElement.nativeElement as HTMLElement).children[0].tagName).toBe(
      'ROUTER-OUTLET',
    );
  });
});
