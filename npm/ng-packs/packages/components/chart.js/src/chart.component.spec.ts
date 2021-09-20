import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { ReplaySubject } from 'rxjs';
import { ChartComponent } from './chart.component';
import * as widgetUtils from './widget-utils';


Object.defineProperty(window, 'getComputedStyle', {
  value: () => ({
    getPropertyValue: prop => {
      return '';
    },
  }),
});

describe('ChartComponent', () => {
  let spectator: SpectatorHost<ChartComponent>;
  const createHost = createHostFactory({ component: ChartComponent });

  beforeEach(() => {
    (widgetUtils as any).chartJsLoaded$ = new ReplaySubject(1);
    spectator = createHost('<abp-chart [data]="data" type="polarArea"></abp-chart>', {
      hostProps: {
        data: {
          datasets: [
            {
              data: [11],
              backgroundColor: ['#FF6384'],
              label: 'My dataset',
            },
          ],
          labels: ['Red'],
        },
      },
    });

    window.ResizeObserver =
    window.ResizeObserver ||
    jest.fn().mockImplementation(() => ({
        disconnect: jest.fn(),
        observe: jest.fn(),
        unobserve: jest.fn(),
    }));
  });

  test('should have a success class by default', done => {
    import('chart.js').then(() => {
      setTimeout(() => {
        expect(spectator.component.chart).toBeTruthy();
        done();
      }, 0);
    });
  });

  describe('#reinit', () => {
    it('should call the destroy method', done => {
      const spy = jest.spyOn(spectator.component.chart, 'destroy');
      spectator.setHostInput({
        data: {
          datasets: [
            {
              data: [12],
              label: 'My dataset',
            },
          ],
          labels: ['Red'],
        },
      });
      spectator.detectChanges();
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 0);
    });
  });

  describe('#refresh', () => {
    it('should call the update method', done => {
      const spy = jest.spyOn(spectator.component.chart, 'update');
      spectator.component.refresh();
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 0);
    });
  });

  describe('#base64Image', () => {
    it('should return the base64 image', done => {

      setTimeout(() => {
        expect(spectator.component.getBase64Image()).toContain('base64');
        done();
      }, 0);
    });
  });
});
