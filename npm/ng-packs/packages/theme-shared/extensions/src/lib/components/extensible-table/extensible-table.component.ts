import {
  ABP,
  ConfigStateService,
  getShortDateFormat,
  getShortDateShortTimeFormat,
  getShortTimeFormat,
  ListService,
  PermissionService,
} from '@abp/ng.core';
import { formatDate } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  InjectFlags,
  InjectionToken,
  Injector,
  Input,
  LOCALE_ID,
  OnChanges,
  SimpleChanges,
  TemplateRef,
  TrackByFunction,
  Type,
} from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ePropType } from '../../enums/props.enum';
import { EntityActionList } from '../../models/entity-actions';
import { EntityProp, EntityPropList } from '../../models/entity-props';
import { PropData } from '../../models/props';
import { ExtensionsService } from '../../services/extensions.service';
import { EXTENSIONS_IDENTIFIER, PROP_DATA_STREAM } from '../../tokens/extensions.token';

const DEFAULT_ACTIONS_COLUMN_WIDTH = 150;

@Component({
  exportAs: 'abpExtensibleTable',
  selector: 'abp-extensible-table',
  templateUrl: './extensible-table.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExtensibleTableComponent<R = any> implements OnChanges {
  protected _actionsText: string;
  @Input()
  set actionsText(value: string) {
    this._actionsText = value;
  }
  get actionsText(): string {
    return this._actionsText ?? (this.actionList.length > 1 ? 'AbpUi::Actions' : '');
  }

  @Input() data: R[];
  @Input() list: ListService;
  @Input() recordsTotal: number;
  @Input() set actionsColumnWidth(width: number) {
    this.setColumnWidths(width ? Number(width) : undefined);
  }
  @Input() actionsTemplate: TemplateRef<any>;

  getInjected: <T>(token: Type<T> | InjectionToken<T>, notFoundValue?: T, flags?: InjectFlags) => T;

  hasAtLeastOnePermittedAction: boolean;

  readonly columnWidths: number[];

  readonly propList: EntityPropList<R>;

  readonly actionList: EntityActionList<R>;

  readonly trackByFn: TrackByFunction<EntityProp<R>> = (_, item) => item.name;

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private config: ConfigStateService,
    private injector: Injector,
  ) {
    this.getInjected = injector.get.bind(injector);
    const extensions = injector.get(ExtensionsService);
    const name = injector.get(EXTENSIONS_IDENTIFIER);
    this.propList = extensions.entityProps.get(name).props;
    this.actionList = extensions['entityActions'].get(name)
      .actions as unknown as EntityActionList<R>;

    const permissionService = injector.get(PermissionService);
    this.hasAtLeastOnePermittedAction =
      permissionService.filterItemsByPolicy(
        this.actionList.toArray().map(action => ({ requiredPolicy: action.permission })),
      ).length > 0;
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

  private getEnum(rowValue: any, list: Array<ABP.Option<any>>) {
    if (!list) return rowValue;
    const { key } = list.find(({ value }) => value === rowValue);
    return key;
  }

  getContent(prop: EntityProp<R>, data: PropData): Observable<string> {
    return prop.valueResolver(data).pipe(
      map(value => {
        switch (prop.type) {
          case ePropType.Boolean:
            return this.getIcon(value);
          case ePropType.Date:
            return this.getDate(value, getShortDateFormat(this.config));
          case ePropType.Time:
            return this.getDate(value, getShortTimeFormat(this.config));
          case ePropType.DateTime:
            return this.getDate(value, getShortDateShortTimeFormat(this.config));
          case ePropType.Enum:
            return this.getEnum(value, prop.enumList);
          default:
            return value;
          // More types can be handled in the future
        }
      }),
    );
  }

  ngOnChanges({ data }: SimpleChanges) {
    if (!data?.currentValue) return;

    this.data = data.currentValue.map((record, index) => {
      this.propList.forEach(prop => {
        const propData = { getInjected: this.getInjected, record, index } as any;
        const value = this.getContent(prop.value, propData);

        const propKey = `_${prop.value.name}`;
        record[propKey] = {
          visible: prop.value.visible(propData),
          value,
        };
        if (prop.value.component) {
          const injector = Injector.create(
            [
              {
                provide: PROP_DATA_STREAM,
                useValue: value,
              },
            ],
            this.injector,
          );
          record[propKey].injector = injector;
          record[propKey].component = prop.value.component;
        }
      });

      return record;
    });
  }
}
