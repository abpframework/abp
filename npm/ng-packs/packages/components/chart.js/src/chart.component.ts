import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  Output
} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { chartJsLoaded$ } from './widget-utils';
declare const Chart: any;

@Component({
  selector: 'abp-chart',
  templateUrl: './chart.component.html',
})
export class ChartComponent implements AfterViewInit, OnDestroy {
  @Input() type: string;

  @Input() options: any = {};

  @Input() plugins: any[] = [];

  @Input() width: string;

  @Input() height: string;

  @Input() responsive = true;

  // eslint-disable-next-line @angular-eslint/no-output-on-prefix
  @Output() readonly onDataSelect: EventEmitter<any> = new EventEmitter();

  @Output() readonly initialized = new BehaviorSubject(this);

  private _initialized: boolean;

  _data: any;

  chart: any;

  constructor(public el: ElementRef, private cdRef: ChangeDetectorRef) {}

  @Input() get data(): any {
    return this._data;
  }

  set data(val: any) {
    this._data = val;
    this.reinit();
  }

  get canvas() {
    return this.el.nativeElement.children[0].children[0];
  }

  get base64Image() {
    return this.chart.toBase64Image();
  }

  ngAfterViewInit() {
    chartJsLoaded$.subscribe(() => {
      this.testChartJs();

      this.initChart();
      this._initialized = true;
    });
  }

  testChartJs() {
    try {
      Chart;
    } catch (error) {
      throw new Error(`Chart is not found. Import the Chart from app.module like shown below:
      import('chart.js');
      `);
    }
  }

  onCanvasClick = event => {
    if (this.chart) {
      const element = this.chart.getElementAtEvent(event);
      const dataset = this.chart.getDatasetAtEvent(event);
      if (element && element.length && dataset) {
        this.onDataSelect.emit({
          originalEvent: event,
          element: element[0],
          dataset,
        });
      }
    }
  };

  initChart = () => {
    const opts = this.options || {};
    opts.responsive = this.responsive;

    // allows chart to resize in responsive mode
    if (opts.responsive && (this.height || this.width)) {
      opts.maintainAspectRatio = false;
    }

    this.chart = new Chart(this.canvas, {
      type: this.type,
      data: this.data,
      options: this.options,
      plugins: this.plugins,
    });

    this.cdRef.detectChanges();
  };

  generateLegend = () => {
    if (this.chart) {
      return this.chart.generateLegend();
    }
  };

  refresh = () => {
    if (this.chart) {
      this.chart.update();
      this.cdRef.detectChanges();
    }
  };

  reinit = () => {
    if (this.chart) {
      this.chart.destroy();
      this.initChart();
    }
  };

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
      this._initialized = false;
      this.chart = null;
    }
  }
}
