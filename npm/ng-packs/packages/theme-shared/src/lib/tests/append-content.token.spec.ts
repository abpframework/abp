import { DomInsertionService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator';
import styles from '../constants/styles';
import { THEME_SHARED_APPEND_CONTENT } from '../tokens/append-content.token';

@Component({ selector: 'abp-dummy', template: '' })
class DummyComponent {}

describe('AppendContentToken', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory(DummyComponent);

  beforeEach(() => (spectator = createComponent()));

  it('should insert a style element to the DOM', () => {
    spectator.inject(THEME_SHARED_APPEND_CONTENT);
    expect(spectator.inject(DomInsertionService).has(styles)).toBe(true);
  });
});
