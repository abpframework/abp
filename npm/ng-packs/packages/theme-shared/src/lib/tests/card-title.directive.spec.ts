import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardTitle } from '../components';

describe('AbpCardTitleDirective', () => {
  let spectator: SpectatorHost<CardTitle>;

  const createHost = createHostFactory(CardTitle);

  beforeEach(() => (spectator = createHost(`<h5 abpCardTitle>CardTitle</h5>`)));

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-title', () => {
    expect(spectator.element.classList).toContain('card-title');
  });
});
