import { createHostFactory, SpectatorHost } from '@ngneat/spectator';
import { CardBodyComponent } from '../components';

describe('AbpCardBodyComponent', () => {
  let spectator: SpectatorHost<CardBodyComponent>;

  const createHost = createHostFactory(CardBodyComponent);

  beforeEach(
    () =>
      (spectator = createHost(
        ` <abp-card-body 
        [cardBodyStyle]="{'background-color':'red'}" 
      > 
      <p>Body</p>
    </abp-card-body>`,
        {
          hostProps: { attributes: { autofocus: '', name: 'abp-card-body' } },
        },
      )),
  );

  it('should create an instance', () => {
    expect(spectator).toBeTruthy();
  });

  it('should have class card-body', () => {
    expect(spectator.element.classList).toContain('card-body');
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

  it('should have p tag with Body text', () => {
    expect(spectator.query('p').textContent).toContain('Body');
  });
});
