import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardTitleDirective } from '../components';

describe('AbpCardTitleDirective', () => {
  let spectator: SpectatorHost<CardTitleDirective>;

  const createHost = createHostFactory(CardTitleDirective);

  beforeEach(() => (spectator = createHost(`<h5 abpCardTitle>CardTitle</h5>`)));

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-title', () => {
    expect(spectator.element.classList).toContain('card-title');
  });
});
