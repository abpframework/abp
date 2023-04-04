import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardHeaderDirective } from '../components';

describe('AbpCardHeaderDirective', () => {
  let spectator: SpectatorHost<CardHeaderDirective>;

  const createHost = createHostFactory(CardHeaderDirective);

  beforeEach(
    () =>
      (spectator = createHost(
        `<div
      abpCardHeader>
    </div>`,
        {
          hostProps: { attributes: { autofocus: '', name: 'abp-card-header' } },
        },
      )),
  );

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-header', () => {
    expect(spectator.element.classList).toContain('card-header');
  });
});
