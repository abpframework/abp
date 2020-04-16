import {
  AfterViewInit,
  ComponentFactory,
  ComponentFactoryResolver,
  Directive,
  TemplateRef,
  ViewContainerRef,
  ComponentRef,
  EmbeddedViewRef,
  Input,
} from '@angular/core';
import { TableComponent } from '../components/table/table.component';

@Directive({ selector: '[abpTableExpandedRow]', exportAs: 'abpTableExpandedRow' })
export class TableExpandedRowDirective implements AfterViewInit {
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

  ngAfterViewInit() {
    this.vcRef.createEmbeddedView(this.template, {
      toggleExpand: this.toggleExpand,
      expanded: this.expanded,
    });
  }

  toggleExpand = () => {
    this.expanded = !this.expanded;

    const colspan = (this.template.elementRef
      .nativeElement as HTMLElement).nextElementSibling.querySelectorAll('td').length;

    const includedKeys = this.includedKeys || this.getIncludedKeys();

    const context = {
      $implicit: this.record,
      colspan,
      includedKeys,
    };

    if (this.expanded) {
      this.expandedRowRef = this.vcRef.createEmbeddedView(
        this.rowTemplate || this.tableRef.expandedRowRef,
        context,
      );
    } else {
      this.expandedRowRef.destroy();
    }
  };

  private getIncludedKeys() {
    const temp = { ...this.record };
    this.excludedKeys.forEach(key => delete temp[key]);

    return Object.keys(temp);
  }
}
