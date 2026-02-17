import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
  ViewChild,
  effect,
  inject,
  input,
  output,
  untracked,
} from '@angular/core';

let Chart: any;

@Component({
  selector: 'abp-chart',
  template: `
    <div
      style="position:relative"
      [style.width]="responsive() && !width() ? null : width()"
      [style.height]="responsive() && !height() ? null : height()"
    >
      <canvas
        #canvas
        [attr.width]="responsive() && !width() ? null : width()"
        [attr.height]="responsive() && !height() ? null : height()"
        (click)="onCanvasClick($event)"
      ></canvas>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  exportAs: 'abpChart',
})
export class ChartComponent implements AfterViewInit, OnDestroy {
  el = inject(ElementRef);
  private cdr = inject(ChangeDetectorRef);

  readonly type = input.required<string>();
  readonly data = input<any>({});
  readonly options = input<any>({});
  readonly plugins = input<any[]>([]);
  readonly width = input<string>();
  readonly height = input<string>();
  readonly responsive = input<boolean>(true);

  readonly dataSelect = output<any>();
  readonly initialized = output<boolean>();

  @ViewChild('canvas') canvas!: ElementRef<HTMLCanvasElement>;

  chart: any;

  constructor() {
    effect(() => {
      const data = this.data();
      const options = this.options();

      untracked(() => {
        if (!this.chart) return;
        this.chart.destroy();
        this.initChart(data, options);
      });
    });
  }

  ngAfterViewInit() {
    import('chart.js/auto').then(module => {
      Chart = module.default;
      this.initChart(this.data(), this.options());
      this.initialized.emit(true);
    });
  }

  onCanvasClick(event: MouseEvent) {
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

  private initChart = (data: any, options: any) => {
    const opts = options || {};
    opts.responsive = this.responsive();

    // allows chart to resize in responsive mode
    if (opts.responsive && (this.height() || this.width())) {
      opts.maintainAspectRatio = false;
    }

    this.chart = new Chart(this.canvas.nativeElement, {
      type: this.type() as any,
      data: data,
      options: opts,
      plugins: this.plugins(),
    });
  };

  getCanvas = () => {
    return this.canvas.nativeElement;
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
    this.initChart(this.data(), this.options());
  };

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
  }
}

