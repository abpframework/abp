import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardHeaderComponent } from '../components';
import { setInputSignal } from './utils';

describe('AbpCardHeaderComponent', () => {
  let spectator: SpectatorHost<CardHeaderComponent>;
  const createHost = createHostFactory(CardHeaderComponent);

  beforeEach(
    () =>
      {
        spectator = createHost(
          `<abp-card-header> Header </abp-card-header>`,
          {
            detectChanges: false,
            hostProps: { attributes: { autofocus: '', name: 'abp-card-header' } },
          },
        );
        setInputSignal(spectator.component.cardHeaderStyle, 'background-color: red;');
        spectator.detectChanges();
      },
  );

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-header', () => {
    expect(spectator.element.classList).toContain('card-header');
  });

  it('should have background-color red', () => {
    expect((spectator.query('div') as HTMLElement).style.backgroundColor).toEqual('red');
  });

  it('should have Header text', () => {
    expect(spectator.element.textContent).toContain('Header');
  });
});
