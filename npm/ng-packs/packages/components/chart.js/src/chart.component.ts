import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  Output,
  SimpleChanges
} from '@angular/core';

let Chart: any;

@Component({
  selector: 'abp-chart',
  template: `
    <div
      style="position:relative"
      [style.width]="responsive && !width ? null : width"
      [style.height]="responsive && !height ? null : height"
    >
      <canvas
        [attr.width]="responsive && !width ? null : width"
        [attr.height]="responsive && !height ? null : height"
        (click)="onCanvasClick($event)"
      ></canvas>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  exportAs: 'abpChart',
})
export class ChartComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() type: string;

  @Input() data: any = {};

  @Input() options: any = {};

  @Input() plugins: any[] = [];

  @Input() width: string;

  @Input() height: string;

  @Input() responsive = true;

  @Output() dataSelect = new EventEmitter();

  @Output() initialized = new EventEmitter<boolean>();

  chart: any;

  constructor(public el: ElementRef, private cdr: ChangeDetectorRef) {}

  ngAfterViewInit() {
    import('chart.js/auto').then(module => {
      Chart = module.default;
      this.initChart();
      this.initialized.emit(true);
    });
  }

  onCanvasClick(event) {
    if (this.chart) {
      const element = this.chart.getElementsAtEventForMode(
        event,
        'nearest',
        { intersect: true },
        false,
      );
      const dataset = this.chart.getElementsAtEventForMode(
        event,
        'dataset',
        { intersect: true },
        false,
      );

      if (element && element[0] && dataset) {
        this.dataSelect.emit({ originalEvent: event, element: element[0], dataset: dataset });
      }
    }
  }

  initChart = () => {
    const opts = this.options || {};
    opts.responsive = this.responsive;

    // allows chart to resize in responsive mode
    if (opts.responsive && (this.height || this.width)) {
      opts.maintainAspectRatio = false;
    }

    this.chart = new Chart(this.el.nativeElement.children[0].children[0], {
      type: this.type as any,
      data: this.data,
      options: this.options,
    });
  };

  getCanvas = () => {
    return this.el.nativeElement.children[0].children[0];
  };

  getBase64Image = () => {
    return this.chart.toBase64Image();
  };

  generateLegend = () => {
    if (this.chart) {
      return this.chart.generateLegend();
    }
  };

  refresh = () => {
    if (this.chart) {
      this.chart.update();
      this.cdr.detectChanges();
    }
  };

  reinit = () => {
    if (!this.chart) return;
    this.chart.destroy();
    this.initChart();
  };

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.chart) return;
    
    if (changes.data?.currentValue || changes.options?.currentValue) {
      this.chart.destroy();
      this.initChart();
    }
  }
}
