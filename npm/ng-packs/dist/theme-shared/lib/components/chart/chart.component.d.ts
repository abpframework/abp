import { AfterViewInit, ElementRef, EventEmitter, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
export declare class ChartComponent implements AfterViewInit, OnDestroy {
  constructor(el: ElementRef, cdRef: ChangeDetectorRef);
  el: ElementRef;
  private cdRef;
  type: string;
  options: any;
  plugins: any[];
  width: string;
  height: string;
  responsive: boolean;
  readonly onDataSelect: EventEmitter<any>;
  readonly initialized: BehaviorSubject<this>;
  private _initialized;
  _data: any;
  chart: any;
  data: any;
  readonly canvas: any;
  readonly base64Image: any;
  onCanvasClick: (event: any) => void;
  initChart: () => void;
  generateLegend: () => any;
  refresh: () => void;
  reinit: () => void;
  ngAfterViewInit(): void;
  ngOnDestroy(): void;
}
