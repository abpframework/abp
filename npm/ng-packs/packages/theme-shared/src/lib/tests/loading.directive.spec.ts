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
    declarations: [LoadingComponent, DummyComponent],
    entryComponents: [LoadingComponent],
  });

  describe('default', () => {
    beforeEach(() => {
      spectator = createDirective('<div [abpLoading]="loading">Testing Loading Directive</div>', {
        hostProps: { loading: true },
      });
    });

    it('should create the loading component', done => {
      setTimeout(() => {
        expect(spectator.directive.rootNode).toBeTruthy();
        expect(spectator.directive.componentRef).toBeTruthy();
        done();
      }, 20);
    });
  });

  describe('with custom target', () => {
    const mockTarget = document.createElement('div');
    const spy = jest.spyOn(mockTarget, 'appendChild');

    beforeEach(() => {
      spectator = createDirective(
        '<div [abpLoading]="loading" [abpLoadingDelay]="delay" [abpLoadingTargetElement]="target">Testing Loading Directive</div>',
        {
          hostProps: { loading: true, target: mockTarget, delay: 0 },
        },
      );
    });

    it('should add the loading component to the DOM', done => {
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 20);
    });

    it('should remove the loading component to the DOM', done => {
      const rendererSpy = jest.spyOn(spectator.directive['renderer'], 'removeChild');
      setTimeout(() => spectator.setHostInput({ loading: false }), 0);
      setTimeout(() => {
        expect(rendererSpy).toHaveBeenCalled();
        expect(spectator.directive.rootNode).toBeFalsy();
        done();
      }, 20);
    });

    it('should appear with delay', done => {
      spectator.setHostInput({ loading: false, delay: 20 });
      spectator.detectChanges();
      setTimeout(() => spectator.setHostInput({ loading: true }), 0);
      setTimeout(() => expect(spectator.directive.loading).toBe(false), 15);
      setTimeout(() => {
        expect(spectator.directive.loading).toBe(true);
        done();
      }, 50);
    });
  });

  describe('with a component selector', () => {
    beforeEach(() => {
      spectator = createDirective('<abp-dummy [abpLoading]="loading"></abp-dummy>', {
        hostProps: { loading: true },
      });
    });

    it('should select the child element', done => {
      setTimeout(() => {
        expect(spectator.directive.targetElement.id).toBe('dummy');
        done();
      }, 20);
    });
  });
});
