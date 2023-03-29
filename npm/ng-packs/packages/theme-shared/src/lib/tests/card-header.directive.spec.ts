import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardHeader } from '../components';

describe('AbpCardHeaderDirective', () => {
  let spectator: SpectatorHost<CardHeader>;

  const createHost = createHostFactory(CardHeader);

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
