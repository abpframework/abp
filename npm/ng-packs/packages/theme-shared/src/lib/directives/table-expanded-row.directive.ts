import {
  Directive,
  EmbeddedViewRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChange,
  SimpleChanges,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import compare from 'just-compare';
import { TableComponent } from '../components/table/table.component';

@Directive({ selector: '[abpTableExpandedRow]', exportAs: 'abpTableExpandedRow' })
export class TableExpandedRowDirective implements OnInit, OnChanges {
  @Input('abpTableExpandedRowRecord')
  record: any;

  @Input('abpTableExpandedRowExcludedKeys')
  excludedKeys: string[];

  @Input('abpTableExpandedRowIncludedKeys')
  includedKeys: string[];

  @Input('abpTableExpandedRowRowTemplate')
  rowTemplate: TemplateRef<any>;

  expanded: boolean;

  expandedRowRef: EmbeddedViewRef<any>;

  constructor(
    private vcRef: ViewContainerRef,
    private template: TemplateRef<any>,
    private tableRef: TableComponent,
  ) {}

  ngOnInit() {
    this.vcRef.createEmbeddedView(this.template, {
      toggleExpand: this.toggleExpand,
      getExpanded: this.getExpanded,
    });
  }

  getExpanded = (): boolean => {
    return this.expanded;
  };

  toggleExpand = () => {
    this.expanded = !this.expanded;

    if (this.expanded) {
      this.createView();
    } else {
      this.expandedRowRef.destroy();
    }
  };

  createView() {
    const colspan = (this.template.elementRef.nativeElement as HTMLElement).parentElement
      .querySelector('tr')
      .querySelectorAll('td').length;

    const includedKeys = this.includedKeys || this.getIncludedKeys();

    const data = includedKeys.reduce(
      (acc, key) => [...acc, { key, value: this.record[key], type: typeof this.record[key] }],
      [],
    );

    const context = {
      $implicit: data,
      colspan,
    };

    this.expandedRowRef = this.vcRef.createEmbeddedView(
      this.rowTemplate || this.tableRef.expandedRowRef,
      context,
    );
  }

  private getIncludedKeys() {
    const temp = { ...this.record };
    this.excludedKeys.forEach(key => delete temp[key]);

    return Object.keys(temp);
  }

  ngOnChanges({ record: { previousValue, currentValue } = {} as SimpleChange }: SimpleChanges) {
    if (previousValue && !compare(previousValue, currentValue) && this.expandedRowRef) {
      this.expandedRowRef.destroy();
      this.createView();
    }
  }
}
