import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardSubtitle } from '../components';

describe('AbpCardSubtitleDirective', () => {
  let spectator: SpectatorHost<CardSubtitle>;

  const createHost = createHostFactory(CardSubtitle);

  beforeEach(() => (spectator = createHost(`<p abpCardSubtitle>CardSubtitle</p>`)));

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-subtitle', () => {
    expect(spectator.element.classList).toContain('card-subtitle');
  });

  it('should have CardSubtitle text', () => {
    expect(spectator.element.textContent).toContain('CardSubtitle');
  });
});
