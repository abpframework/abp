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
      spectator = createDirective('<div [abpLoading]="status">Testing Loading Directive</div>', {
        hostProps: { status: true },
      });
    });

    it('should create the loading component', done => {
      setTimeout(() => {
        expect(spectator.directive.rootNode).toBeTruthy();
        expect(spectator.directive.componentRef).toBeTruthy();
        done();
      }, 0);
    });
  });

  describe('with custom target', () => {
    const mockTarget = document.createElement('div');
    const spy = jest.spyOn(mockTarget, 'appendChild');

    beforeEach(() => {
      spectator = createDirective(
        '<div [abpLoading]="status" [abpLoadingTargetElement]="target">Testing Loading Directive</div>',
        {
          hostProps: { status: true, target: mockTarget },
        },
      );
    });

    it('should add the loading component to the DOM', done => {
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 0);
    });

    it('should remove the loading component to the DOM', done => {
      const rendererSpy = jest.spyOn(spectator.directive['renderer'], 'removeChild');
      spectator.setHostInput({ status: false });
      setTimeout(() => {
        expect(rendererSpy).toHaveBeenCalled();
        expect(spectator.directive.rootNode).toBeFalsy();
        done();
      }, 0);
    });
  });

  describe('with a component selector', () => {
    beforeEach(() => {
      spectator = createDirective('<abp-dummy [abpLoading]="status"></abp-dummy>', {
        hostProps: { status: true },
      });
    });

    it('should select the child element', done => {
      setTimeout(() => {
        expect(spectator.directive.targetElement.id).toBe('dummy');
        done();
      }, 0);
    });
  });
});
