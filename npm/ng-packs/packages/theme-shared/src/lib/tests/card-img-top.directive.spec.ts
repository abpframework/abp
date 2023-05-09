import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardImgTopDirective } from '../components';

describe('AbpCardImgTopDirective', () => {
  let spectator: SpectatorHost<CardImgTopDirective>;

  const createHost = createHostFactory(CardImgTopDirective);

  beforeEach(
    () =>
      (spectator = createHost(
        `<img
      abpCardImgTop
    />`,
      )),
  );

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-img-top', () => {
    expect(spectator.element.classList).toContain('card-img-top');
  });
});
