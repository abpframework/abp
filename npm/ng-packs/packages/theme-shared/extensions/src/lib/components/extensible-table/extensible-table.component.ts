import { ListService } from '@abp/ng.core';
import { formatDate } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  Injector,
  Input,
  LOCALE_ID,
  TemplateRef,
  TrackByFunction,
} from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ePropType } from '../../enums/props.enum';
import { EntityProp, EntityPropList } from '../../models/entity-props';
import { PropData } from '../../models/props';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_IDENTIFIER } from '../../tokens/extensions.token';
const DEFAULT_ACTIONS_COLUMN_WIDTH = 150;

@Component({
  exportAs: 'abpExtensibleTable',
  selector: 'abp-extensible-table',
  templateUrl: './extensible-table.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExtensibleTableComponent<R = any> {
  @Input() actionsText: string;
  @Input() data: R[];
  @Input() list: ListService;
  @Input() recordsTotal: number;
  @Input() set actionsColumnWidth(width: number) {
    this.setColumnWidths(width ? Number(width) : undefined);
  }
  @Input() actionsTemplate: TemplateRef<any>;

  readonly columnWidths: number[];

  readonly propList: EntityPropList<R>;

  readonly trackByFn: TrackByFunction<EntityProp<R>> = (_, item) => item.name;

  constructor(@Inject(LOCALE_ID) private locale: string, injector: Injector) {
    const extensions = injector.get(ExtensionsService);
    const name = injector.get(EXTENSIONS_IDENTIFIER);
    this.propList = extensions.entityProps.get(name).props;
    this.setColumnWidths(DEFAULT_ACTIONS_COLUMN_WIDTH);
  }

  private setColumnWidths(actionsColumn: number) {
    const widths = [actionsColumn];
    this.propList.forEach(({ value: prop }) => {
      widths.push(prop.columnWidth);
    });
    (this.columnWidths as any) = widths;
  }

  private getDate(value: Date, format: string) {
    return value ? formatDate(value, format, this.locale) : '';
  }

  private getIcon(value: boolean) {
    return value
      ? '<div class="text-center text-success"><i class="fa fa-check"></i></div>'
      : '<div class="text-center text-danger"><i class="fa fa-times"></i></div>';
  }

  getContent(prop: EntityProp<R>, data: PropData): Observable<string> {
    return prop.valueResolver(data).pipe(
      map(value => {
        switch (prop.type) {
          case ePropType.Boolean:
            return this.getIcon(value);
          case ePropType.Date:
            return this.getDate(value, 'yyyy-MM-dd');
          case ePropType.Time:
            return this.getDate(value, 'HH:mm');
          case ePropType.DateTime:
            return this.getDate(value, 'yyyy-MM-dd HH:mm:ss Z');
          default:
            return value;
          // More types can be handled in the future
        }
      }),
    );
  }
}
