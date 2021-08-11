import { SpectatorDirective, createDirectiveFactory } from '@ngneat/spectator/jest';
import { VisibilityDirective } from '../directives/visibility.directive';

describe('VisibilityDirective', () => {
  let spectator: SpectatorDirective<VisibilityDirective>;
  let directive: VisibilityDirective;
  const createDirective = createDirectiveFactory({
    directive: VisibilityDirective,
  });

  describe('without content', () => {
    beforeEach(() => {
      spectator = createDirective('<div [abpVisibility]></div>');
      directive = spectator.directive;
    });

    it('should be created', () => {
      expect(directive).toBeTruthy();
    });

    xit('should be removed', done => {
      setTimeout(() => {
        expect(spectator.query('div')).toBeFalsy();
        done();
      }, 0);
    });
  });

  describe('without mutation observer and with content', () => {
    beforeEach(() => {
      spectator = createDirective('<div [abpVisibility]><p id="content">Content</p></div>');
      directive = spectator.directive;
    });

    it('should not removed', done => {
      setTimeout(() => {
        expect(spectator.query('div')).toBeTruthy();
        done();
      }, 0);
    });
  });

  describe('without mutation observer and with focused element', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<div id="main" [abpVisibility]="container"></div><div #container><p id="content">Content</p></div>',
      );
      directive = spectator.directive;
    });

    it('should not removed', done => {
      setTimeout(() => {
        expect(spectator.query('#main')).toBeTruthy();
        done();
      }, 0);
    });
  });

  describe('without content and with focused element', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<div id="main" [abpVisibility]="container"></div><div #container></div>',
      );
      directive = spectator.directive;
    });

    xit('should be removed', done => {
      setTimeout(() => {
        expect(spectator.query('#main')).toBeFalsy();
        done();
      }, 0);
    });
  });

  describe('with mutation observer and with content', () => {
    beforeEach(() => {
      spectator = createDirective('<div [abpVisibility]><div id="content">Content</div></div>');
      directive = spectator.directive;
    });

    xit('should remove the main div element when content removed', done => {
      spectator.query('#content').remove();

      setTimeout(() => {
        expect(spectator.query('div')).toBeFalsy();
        done();
      }, 0);
    });

    it('should not remove the main div element', done => {
      spectator.query('div').appendChild(document.createElement('div'));

      setTimeout(() => {
        expect(spectator.query('div')).toBeTruthy();
        done();
      }, 100);
    });
  });

  describe('with mutation observer and with focused element', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<div id="main" [abpVisibility]="container"></div><div #container><p id="content">Content</p></div>',
      );
      directive = spectator.directive;
    });

    xit('should remove the main div element when content removed', done => {
      spectator.query('#content').remove();

      setTimeout(() => {
        expect(spectator.query('#main')).toBeFalsy();
        done();
      }, 0);
    });

    it('should not remove the main div element', done => {
      spectator.query('#content').appendChild(document.createElement('div'));

      setTimeout(() => {
        expect(spectator.query('#main')).toBeTruthy();
        done();
      }, 100);
    });
  });
});
