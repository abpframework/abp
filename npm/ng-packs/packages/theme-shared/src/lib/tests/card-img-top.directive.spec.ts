import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardImgTop } from '../components';

describe('AbpCardImgTopDirective', () => {
  let spectator: SpectatorHost<CardImgTop>;

  const createHost = createHostFactory(CardImgTop);

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
