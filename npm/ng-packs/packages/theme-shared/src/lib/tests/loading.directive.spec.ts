import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/jest';
import { LoadingDirective } from '../directives';
import { LoadingComponent } from '../components';
import { Component } from '@angular/core';

@Component({
  selector: 'abp-dummy',
  template: '<div id="dummy">Testing Loading Directive</div>',
})
export class DummyComponent {}

describe('LoadingDirective', () => {
  let spectator: SpectatorDirective<LoadingDirective>;
  const createDirective = createDirectiveFactory({
    directive: LoadingDirective,
    declarations: [],
    entryComponents: [],
    imports: [LoadingComponent, DummyComponent],
  });

  describe('default', () => {
    beforeEach(() => {
      spectator = createDirective('<div [abpLoading]="loading">Testing Loading Directive</div>', {
        hostProps: { loading: true },
      });
    });

    it('should create directive', () => {
      expect(spectator.directive).toBeTruthy();
    });

    it('should handle loading input', () => {
      spectator.setHostInput({ loading: false });
      spectator.detectChanges();
      expect(spectator.directive).toBeTruthy();
    });
  });

  describe('with custom target', () => {
    const mockTarget = document.createElement('div');

    beforeEach(() => {
      spectator = createDirective(
        '<div [abpLoading]="loading" [abpLoadingDelay]="delay" [abpLoadingTargetElement]="target">Testing Loading Directive</div>',
        {
          hostProps: { loading: true, target: mockTarget, delay: 0 },
        },
      );
    });

    it('should create directive with custom target', () => {
      expect(spectator.directive).toBeTruthy();
      expect(spectator.directive.targetElement).toBe(mockTarget);
    });

    it('should handle delay input', () => {
      spectator.setHostInput({ delay: 100 });
      spectator.detectChanges();
      expect(spectator.directive).toBeTruthy();
    });

    it('should handle loading state changes', () => {
      spectator.setHostInput({ loading: false });
      spectator.detectChanges();
      expect(spectator.directive).toBeTruthy();
      
      spectator.setHostInput({ loading: true });
      spectator.detectChanges();
      expect(spectator.directive).toBeTruthy();
    });
  });

  describe('with a component selector', () => {
    beforeEach(() => {
      spectator = createDirective('<abp-dummy [abpLoading]="loading"></abp-dummy>', {
        hostProps: { loading: true },
      });
    });

    it('should create directive with component selector', () => {
      expect(spectator.directive).toBeTruthy();
    });

    it('should have target element', () => {
      expect(spectator.directive.targetElement).toBeDefined();
    });
  });
});
