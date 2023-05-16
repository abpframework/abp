import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardFooterComponent } from '../components';

describe('AbpCardFooterComponent', () => {
  let spectator: SpectatorHost<CardFooterComponent>;

  const createHost = createHostFactory(CardFooterComponent);

  beforeEach(
    () =>
      (spectator = createHost(
        `<abp-card-footer 
      [cardFooterStyle]="{'background-color':'red'}" 
    > 
    <p>Footer</p>
  </abp-card-footer>`,
        {
          hostProps: { attributes: { autofocus: '', name: 'abp-card-footer' } },
        },
      )),
  );

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-footer', () => {
    expect(spectator.element.classList).toContain('card-footer');
  });

  it('should have div', () => {
    expect(spectator.query('div')).toBeTruthy();
  });

  it('should have background-color red', () => {
    expect((spectator.query('div') as HTMLElement).style.backgroundColor).toEqual('red');
  });

  it('should have p tag', () => {
    expect(spectator.query('p')).toBeTruthy();
  });

  it('should have p tag with Footer text', () => {
    expect((spectator.query('p') as HTMLElement).textContent).toContain('Footer');
  });
});
