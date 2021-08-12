import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { ChartComponent } from '../components';
import { chartJsLoaded$ } from '../utils/widget-utils';
import { ReplaySubject } from 'rxjs';
import * as widgetUtils from '../utils/widget-utils';
// import 'chart.js';
declare const Chart;

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
  });

  test('should throw error when chart.js is not loaded', () => {
    try {
      spectator.component.testChartJs();
    } catch (error) {
      expect(error.message).toContain('Chart is not found');
    }
  });

  test('should have a success class by default', async done => {
    await import('chart.js');

    chartJsLoaded$.next();
    setTimeout(() => {
      expect(spectator.component.chart).toBeTruthy();
      done();
    }, 0);
  });

  describe('#reinit', () => {
    it('should call the destroy method', done => {
      chartJsLoaded$.next();
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
      chartJsLoaded$.next();
      const spy = jest.spyOn(spectator.component.chart, 'update');
      spectator.component.refresh();
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 0);
    });
  });

  describe('#generateLegend', () => {
    it('should call the generateLegend method', done => {
      chartJsLoaded$.next();
      const spy = jest.spyOn(spectator.component.chart, 'generateLegend');
      spectator.component.generateLegend();
      setTimeout(() => {
        expect(spy).toHaveBeenCalled();
        done();
      }, 0);
    });
  });

  describe('#onCanvasClick', () => {
    it('should emit the onDataSelect', done => {
      spectator.component.onDataSelect.subscribe(() => {
        done();
      });

      chartJsLoaded$.next();
      jest.spyOn(spectator.component.chart, 'getElementAtEvent').mockReturnValue([document.createElement('div')]);
      spectator.click('canvas');
    });
  });

  describe('#base64Image', () => {
    it('should return the base64 image', done => {
      chartJsLoaded$.next();

      setTimeout(() => {
        expect(spectator.component.base64Image).toContain('base64');
        done();
      }, 0);
    });
  });
});
