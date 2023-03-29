import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import {
  CardComponent,
  CardBodyComponent,
  CardFooterComponent,
  CardHeaderComponent,
  CardHeader,
  CardTitle,
  CardImgTop,
  CardSubtitle,
} from '../components/card';

describe('CardComponent', () => {
  let spectator: SpectatorHost<CardComponent>;

  const createHost = createHostFactory({
    component: CardComponent,
    declarations: [
      CardHeaderComponent,
      CardTitle,
      CardSubtitle,
      CardBodyComponent,
      CardImgTop,
      CardFooterComponent,
    ],
  });

  beforeEach(
    () =>
      (spectator = createHost(
        `
        <abp-card>
          <abp-card-header>
            <abp-card-title>Card title</abp-card-title>
            <p abp-card-subtitle>Card subtitle</p>  
          </abp-card-header>
          <abp-card-body>
            <abp-card-img-top src="https://picsum.photos/200/300"></abp-card-img-top>
            <p abpCardSubtitle>Some quick example text to build on the card title and make up the bulk of the card's content.</p>
          </abp-card-body>
          <abp-card-footer>
            <a href="#" class="btn btn-primary">Go somewhere</a>
          </abp-card-footer>
        </abp-card>
        `,
        {
          hostProps: { attributes: { autofocus: '', name: 'abp-card' } },
        },
      )),
  );

  it('should display the card-header', () => {
    expect(spectator.query('abp-card-header')).toBeTruthy();
  });

  it('should display the card-title', () => {
    expect(spectator.query('abp-card-title')).toBeTruthy();
  });

  it('should display the card-subtitle', () => {
    expect(spectator.query('p[abp-card-subtitle]')).toBeTruthy();
  });

  it('should display the card-body', () => {
    expect(spectator.query('abp-card-body')).toBeTruthy();
  });

  it('should display the card-img-top', () => {
    expect(spectator.query('abp-card-img-top')).toBeTruthy();
  });

  it('should display the card-footer', () => {
    expect(spectator.query('abp-card-footer')).toBeTruthy();
  });
});
